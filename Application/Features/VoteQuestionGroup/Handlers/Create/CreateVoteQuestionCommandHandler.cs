using Domain.Common.Results;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Features.VoteQuestionGroup.Handlers.Create;

public class CreateVoteQuestionCommandHandler
    (
        IUnitOfWork repo,
        ILogger<CreateVoteQuestionCommandHandler> logger,
        IValidator<CreateVoteQuestionCommand> validator,
        IHttpContextAccessor context
    ) : IRequestHandler<CreateVoteQuestionCommand, Result<Created>>
{
    public async Task<Result<Created>> Handle(CreateVoteQuestionCommand request, CancellationToken cancellationToken)
    {
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        if (validatorResult.IsValid)
        {
            validatorResult.Errors.MapFromFluentValidationErrors();
        }
        logger.LogInformation("Creating vote question for VoteSessionId {VoteSessionId}", request.VoteSessionId);

        var voteSessionResult = await repo.VoteSessionRepository.GetByIdAsync(request.VoteSessionId);
        if (voteSessionResult.IsError || voteSessionResult.Value == null)
        {
            logger.LogWarning("No vote session found with ID {VoteSessionId}", request.VoteSessionId);
            return Error.NotFound(description: $"No vote session with ID {request.VoteSessionId} was found");
        }

        var voteSession = voteSessionResult.Value;
        var voteQuestion = new VoteQuestion
        {
            StartedAt = request.StartedAt,
            EndedAt = request.EndedAt,
            Title = request.Title,
            VoteSessionId = request.VoteSessionId,
            Options = []
        };

        var addQuestionResult = await repo.VoteQuestionRepository.AddAsync(voteQuestion);
        if (addQuestionResult.IsError)
        {
            logger.LogError("Failed to add vote question: {Errors}", addQuestionResult.Errors);
            return addQuestionResult.Errors;
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
            List<VoteQuestionOption> voteQuestionOptions = [.. request.Options.Select(x => new VoteQuestionOption
            {
                VoteQuestionId = voteQuestion.Id,
                Title = x.Title,
            })];
            logger.LogInformation("Adding {OptionCount} vote question options for VoteQuestionId {VoteQuestionId}", voteQuestionOptions.Count, voteQuestion.Id);

            var addOptionsResult = await repo.VoteQuestionOptionRepository.AddRangeAsync(voteQuestionOptions);
            if (addOptionsResult.IsError)
            {
                logger.LogError("Failed to add vote question options: {Errors}", addOptionsResult.Errors);
                return addOptionsResult.Errors;
            }

            var saveOptionsResult = await repo.SaveChangesAsync(cancellationToken);
            if (saveOptionsResult.IsError)
            {
                logger.LogError("Failed to save vote question options: {Errors}", saveOptionsResult.Errors);
                return saveOptionsResult.Errors;
            }
        }

        // Log the vote question creation
        var systemLog = new SystemLog
        {
            Action = $"Vote question '{request.Title}' created for VoteSessionId {request.VoteSessionId}",
            PerformedBy = performedBy,
            TimeStamp = DateTime.UtcNow
        };
        await repo.SystemLogRepository.AddAsync(systemLog);
        await repo.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Vote question created successfully for VoteSessionId {VoteSessionId}", request.VoteSessionId);
        return Result.Created;
    }
}