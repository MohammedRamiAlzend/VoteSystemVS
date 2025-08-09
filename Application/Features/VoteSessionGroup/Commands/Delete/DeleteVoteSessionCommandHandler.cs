using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Domain.Entities;

namespace Application.Features.VoteSessionGroup.Commands.Delete;

public class DeleteVoteSessionCommandHandler
    (
        IUnitOfWork repo,
        ILogger<DeleteVoteSessionCommandHandler> logger,
        IHttpContextAccessor context
    ) : IRequestHandler<DeleteVoteSessionCommand, Result<Deleted>>
{
    public async Task<Result<Deleted>> Handle(DeleteVoteSessionCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting vote session with ID {VoteSessionId}", request.Id);

        var voteSessionResult = await repo.VoteSessionRepository.GetByIdAsync(request.Id);
        if (voteSessionResult.IsError || voteSessionResult.Value == null)
        {
            logger.LogWarning("No vote session found with ID {VoteSessionId}", request.Id);
            return Error.NotFound(description: $"No vote session with ID {request.Id} was found");
        }

        var deleteResult = await repo.VoteSessionRepository.Remove(voteSessionResult.Value);
        if (deleteResult.IsError)
        {
            logger.LogError("Failed to delete vote session: {Errors}", deleteResult.Errors);
            return deleteResult.Errors;
        }

        var saveResult = await repo.SaveChangesAsync(cancellationToken);
        if (saveResult.IsError)
        {
            logger.LogError("Failed to save changes after deleting vote session: {Errors}", saveResult.Errors);
            return saveResult.Errors;
        }

        // Get admin from context to log action
        var getAdminFromContext = context.HttpContext?.User;
        string performedBy = "Unknown Admin";
        if (getAdminFromContext != null && (getAdminFromContext.IsInRole("Admin") || getAdminFromContext.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin")))
        {
            performedBy = getAdminFromContext.Identity.Name ?? "Unknown Admin";
        }

        // Log the vote session deletion
        var systemLog = new SystemLog
        {
            Action = $"Vote session with ID {request.Id} deleted",
            PerformedBy = performedBy,
            TimeStamp = DateTime.UtcNow
        };
        await repo.SystemLogRepository.AddAsync(systemLog);
        await repo.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully deleted vote session with ID {VoteSessionId}", request.Id);
        return Result.Deleted;
    }
}