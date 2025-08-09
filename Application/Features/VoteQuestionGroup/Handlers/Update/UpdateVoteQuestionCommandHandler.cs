using Domain.Common.Results;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Features.VoteQuestionGroup.Handlers.Update;

public class UpdateVoteQuestionCommandHandler
    (
        IUnitOfWork repo,
        ILogger<UpdateVoteQuestionCommandHandler> logger,
        IValidator<UpdateVoteQuestionCommand> validator,
        IHttpContextAccessor context
    ) : IRequestHandler<UpdateVoteQuestionCommand, Result<Updated>>
{
    public async Task<Result<Updated>> Handle(UpdateVoteQuestionCommand request, CancellationToken cancellationToken)
    {
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validatorResult.IsValid)
        {
            validatorResult.Errors.MapFromFluentValidationErrors();
        }

        logger.LogInformation("Updating vote question with ID {VoteQuestionId}", request.Id);

        var voteQuestionResult = await repo.VoteQuestionRepository.GetByIdAsync(request.Id);
        if (voteQuestionResult.IsError || voteQuestionResult.Value == null)
        {
            logger.LogWarning("No vote question found with ID {VoteQuestionId}", request.Id);
            return Error.NotFound(description: $"No vote question with ID {request.Id} was found");
        }

        var voteQuestion = voteQuestionResult.Value;
        voteQuestion.Title = request.Title;
        voteQuestion.StartedAt = request.StartedAt;
        voteQuestion.EndedAt = request.EndedAt;

        var updateQuestionResult = await repo.VoteQuestionRepository.UpdateAsync(voteQuestion);
        if (updateQuestionResult.IsError)
        {
            logger.LogError("Failed to update vote question: {Errors}", updateQuestionResult.Errors);
            return updateQuestionResult.Errors;
        }

        var saveChangesResult = await repo.SaveChangesAsync(cancellationToken);
        if (saveChangesResult.IsError)
        {
            logger.LogError("Failed to save vote question changes: {Errors}", saveChangesResult.Errors);
            return saveChangesResult.Errors;
        }

        // Get admin from context to log action
        var getAdminFromContext = context.HttpContext?.User;
        string performedBy = "Unknown Admin";
        if (getAdminFromContext != null && (getAdminFromContext.IsInRole("Admin") || getAdminFromContext.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin")))
        {
            performedBy = getAdminFromContext.Identity.Name ?? "Unknown Admin";
        }

        if (request.Options.Count > 0)
        {
            foreach (var optionUpdate in request.Options)
            {
                var existingOption = await repo.VoteQuestionOptionRepository.GetByIdAsync(optionUpdate.Id);
                if (existingOption.IsSuccess && existingOption.Value != null)
                {
                    existingOption.Value.Title = optionUpdate.Title;
                    var updateOptionResult = await repo.VoteQuestionOptionRepository.UpdateAsync(existingOption.Value);
                    if (updateOptionResult.IsError)
                    {
                        logger.LogError("Failed to update option {OptionId}: {Errors}", optionUpdate.Id, updateOptionResult.Errors);
                        return updateOptionResult.Errors;
                    }
                }
                else
                {
                    var newOption = new VoteQuestionOption
                    {
                        VoteQuestionId = voteQuestion.Id,
                        Title = optionUpdate.Title
                    };
                    var addOptionResult = await repo.VoteQuestionOptionRepository.AddAsync(newOption);
                    if (addOptionResult.IsError)
                    {
                        logger.LogError("Failed to add new option: {Errors}", addOptionResult.Errors);
                        return addOptionResult.Errors;
                    }
                }
            }
            var saveOptionsResult = await repo.SaveChangesAsync(cancellationToken);
            if (saveOptionsResult.IsError)
            {
                logger.LogError("Failed to save vote question options: {Errors}", saveOptionsResult.Errors);
                return saveOptionsResult.Errors;
            }
        }

        // Log the vote question update
        var systemLog = new SystemLog
        {
            Action = $"Vote question with ID {request.Id} updated",
            PerformedBy = performedBy,
            TimeStamp = DateTime.UtcNow
        };
        await repo.SystemLogRepository.AddAsync(systemLog);
        await repo.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Vote question with ID {VoteQuestionId} updated successfully", request.Id);
        return Result.Updated;
    }
}