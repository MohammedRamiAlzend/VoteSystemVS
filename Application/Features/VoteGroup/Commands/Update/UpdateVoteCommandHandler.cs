using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Domain.Entities;

namespace Application.Features.VoteGroup.Commands.Update;

public class UpdateVoteCommandHandler
    (
        IUnitOfWork repo,
        ILogger<UpdateVoteCommandHandler> logger,
        IValidator<UpdateVoteCommand> validator,
        IHttpContextAccessor context
    ) : IRequestHandler<UpdateVoteCommand, Result<Updated>>
{
    public async Task<Result<Updated>> Handle(UpdateVoteCommand request, CancellationToken cancellationToken)
    {
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validatorResult.IsValid)
        {
            validatorResult.Errors.MapFromFluentValidationErrors();
        }

        logger.LogInformation("Updating vote with ID {VoteId}", request.Id);

        var voteResult = await repo.VoteRepository.GetByIdAsync(request.Id);
        if (voteResult.IsError || voteResult.Value == null)
        {
            logger.LogWarning("No vote found with ID {VoteId}", request.Id);
            return Error.NotFound(description: $"No vote with ID {request.Id} was found");
        }

        var userExist = await repo.UserRepository.AnyAsync(u => u.Id == request.UserId);
        if (userExist.IsError)
        {
            logger.LogWarning("User with ID {UserId} not found", request.UserId);
            return Error.NotFound(description: $"User with ID {request.UserId} was not found");
        }

        var voteQuestionOptionExist = await repo.VoteQuestionOptionRepository.AnyAsync(vqo => vqo.Id == request.VoteQuestionOptionId);
        if (voteQuestionOptionExist.IsError)
        {
            logger.LogWarning("VoteQuestionOption with ID {VoteQuestionOptionId} not found", request.VoteQuestionOptionId);
            return Error.NotFound(description: $"VoteQuestionOption with ID {request.VoteQuestionOptionId} was not found");
        }

        var vote = voteResult.Value;
        vote.UserId = request.UserId;
        vote.VoteQuestionOptionId = request.VoteQuestionOptionId;

        var updateResult = await repo.VoteRepository.UpdateAsync(vote);
        if (updateResult.IsError)
        {
            logger.LogError("Failed to update vote: {Errors}", updateResult.Errors);
            return updateResult.Errors;
        }

        var saveResult = await repo.SaveChangesAsync(cancellationToken);
        if (saveResult.IsError)
        {
            logger.LogError("Failed to save changes after updating vote: {Errors}", saveResult.Errors);
            return saveResult.Errors;
        }

        // Get user from context to log action
        string performedBy = "Unknown User";
        var getUserFromContext = context.HttpContext?.User;
        if (getUserFromContext != null)
        {
            performedBy = getUserFromContext.Identity.Name ?? getUserFromContext.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown User";
        }

        // Log the vote update
        var systemLog = new SystemLog
        {
            Action = $"Vote with ID {request.Id} updated",
            PerformedBy = performedBy,
            TimeStamp = DateTime.UtcNow
        };
        await repo.SystemLogRepository.AddAsync(systemLog);
        await repo.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully updated vote with ID {VoteId}", request.Id);
        return Result.Updated;
    }
}