using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Domain.Entities;

namespace Application.Features.VoteQuestionGroup.Handlers.Delete;

public class DeleteVoteQuestionCommandHandler
    (
        IUnitOfWork repo,
        ILogger<DeleteVoteQuestionCommandHandler> logger,
        IHttpContextAccessor context
    ) : IRequestHandler<DeleteVoteQuestionCommand, Result<Deleted>>
{
    public async Task<Result<Deleted>> Handle(DeleteVoteQuestionCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting vote question with ID {VoteQuestionId}", request.Id);

        var voteQuestionResult = await repo.VoteQuestionRepository.GetByIdAsync(request.Id);
        if (voteQuestionResult.IsError || voteQuestionResult.Value == null)
        {
            logger.LogWarning("No vote question found with ID {VoteQuestionId}", request.Id);
            return Error.NotFound(description: $"No vote question with ID {request.Id} was found");
        }

        var deleteResult = await repo.VoteQuestionRepository.Remove(voteQuestionResult.Value);
        if (deleteResult.IsError)
        {
            logger.LogError("Failed to delete vote question: {Errors}", deleteResult.Errors);
            return deleteResult.Errors;
        }

        var saveResult = await repo.SaveChangesAsync(cancellationToken);
        if (saveResult.IsError)
        {
            logger.LogError("Failed to save changes after deleting vote question: {Errors}", saveResult.Errors);
            return saveResult.Errors;
        }

        // Get admin from context to log action
        var getAdminFromContext = context.HttpContext?.User;
        string performedBy = "Unknown Admin";
        if (getAdminFromContext != null && (getAdminFromContext.IsInRole("Admin") || getAdminFromContext.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin")))
        {
            performedBy = getAdminFromContext.Identity.Name ?? "Unknown Admin";
        }

        // Log the vote question deletion
        var systemLog = new SystemLog
        {
            Action = $"Vote question with ID {request.Id} deleted",
            PerformedBy = performedBy,
            TimeStamp = DateTime.UtcNow
        };
        await repo.SystemLogRepository.AddAsync(systemLog);
        await repo.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully deleted vote question with ID {VoteQuestionId}", request.Id);
        return Result.Deleted;
    }
}