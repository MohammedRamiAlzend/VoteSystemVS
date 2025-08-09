using Domain.Common.Results;
using Domain.Entities;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Features.VoteGroup.Commands.Create;

public class CreateVoteCommandHandler
    (
        IUnitOfWork repo,
        ILogger<CreateVoteCommandHandler> logger,
        IValidator<CreateVoteCommand> validator,
        IHttpContextAccessor context
    ) : IRequestHandler<CreateVoteCommand, Result<Created>>
{
    public async Task<Result<Created>> Handle(CreateVoteCommand request, CancellationToken cancellationToken)
    {
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validatorResult.IsValid)
        {
            validatorResult.Errors.MapFromFluentValidationErrors();
        }

        logger.LogInformation("Creating vote for UserId {UserId} and VoteQuestionOptionId {VoteQuestionOptionId}", request.UserId, request.VoteQuestionOptionId);

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

        var vote = new Vote
        {
            UserId = request.UserId,
            VoteQuestionOptionId = request.VoteQuestionOptionId,
            VotedAt = DateTime.UtcNow
        };

        var addingResult = await repo.VoteRepository.AddAsync(vote);
        if (addingResult.IsError)
        {
            logger.LogError("Failed to add vote: {Errors}", addingResult.Errors);
            return addingResult.Errors;
        }

        var commit = await repo.SaveChangesAsync(cancellationToken);
        if (commit.IsError)
        {
            logger.LogError("Failed to save changes: {Errors}", commit.Errors);
            return commit.Errors;
        }

        // Get user from context to log action
        string performedBy = "Unknown User";
        var getUserFromContext = context.HttpContext?.User;
        if (getUserFromContext != null)
        {
            performedBy = getUserFromContext.Identity.Name ?? getUserFromContext.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown User";
        }

        // Log the vote creation
        var systemLog = new SystemLog
        {
            Action = $"Vote created for UserId {request.UserId} on Option {request.VoteQuestionOptionId}",
            PerformedBy = performedBy,
            TimeStamp = DateTime.UtcNow
        };
        await repo.SystemLogRepository.AddAsync(systemLog);
        await repo.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Vote created successfully for UserId {UserId} and VoteQuestionOptionId {VoteQuestionOptionId}", request.UserId, request.VoteQuestionOptionId);
        return Result.Created;
    }
}