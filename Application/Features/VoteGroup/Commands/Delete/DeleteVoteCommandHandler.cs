using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Domain.Entities;

namespace Application.Features.VoteGroup.Commands.Delete;

public class DeleteVoteCommandHandler
    (
        IUnitOfWork repo,
        ILogger<DeleteVoteCommandHandler> logger,
        IHttpContextAccessor context
    ) : IRequestHandler<DeleteVoteCommand, Result<Deleted>>
{
    public async Task<Result<Deleted>> Handle(DeleteVoteCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting vote with ID {VoteId}", request.Id);

        var voteResult = await repo.VoteRepository.GetByIdAsync(request.Id);
        if (voteResult.IsError || voteResult.Value == null)
        {
            logger.LogWarning("No vote found with ID {VoteId}", request.Id);
            return Error.NotFound(description: $"No vote with ID {request.Id} was found");
        }

        var deleteResult = await repo.VoteRepository.Remove(voteResult.Value);
        if (deleteResult.IsError)
        {
            logger.LogError("Failed to delete vote: {Errors}", deleteResult.Errors);
            return deleteResult.Errors;
        }

        var saveResult = await repo.SaveChangesAsync(cancellationToken);
        if (saveResult.IsError)
        {
            logger.LogError("Failed to save changes after deleting vote: {Errors}", saveResult.Errors);
            return saveResult.Errors;
        }

        // Get user from context to log action
        string performedBy = "Unknown User";
        var getUserFromContext = context.HttpContext?.User;
        if (getUserFromContext != null)
        {
            performedBy = getUserFromContext.Identity.Name ?? getUserFromContext.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown User";
        }

        // Log the vote deletion
        var systemLog = new SystemLog
        {
            Action = $"Vote with ID {request.Id} deleted",
            PerformedBy = performedBy,
            TimeStamp = DateTime.UtcNow
        };
        await repo.SystemLogRepository.AddAsync(systemLog);
        await repo.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully deleted vote with ID {VoteId}", request.Id);
        return Result.Deleted;
    }
}