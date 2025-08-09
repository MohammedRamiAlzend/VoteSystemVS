using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Domain.Entities;

namespace Application.Features.VoteQuestionOptionGroup.Handlers.Delete;

public class DeleteVoteQuestionOptionCommandHandler
    (
        IUnitOfWork repo,
        ILogger<DeleteVoteQuestionOptionCommandHandler> logger,
        IHttpContextAccessor context
    ) : IRequestHandler<DeleteVoteQuestionOptionCommand, Result<Deleted>>
{
    public async Task<Result<Deleted>> Handle(DeleteVoteQuestionOptionCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting vote question option with ID {VoteQuestionOptionId}", request.Id);

        var voteQuestionOptionResult = await repo.VoteQuestionOptionRepository.GetByIdAsync(request.Id);
        if (voteQuestionOptionResult.IsError || voteQuestionOptionResult.Value == null)
        {
            logger.LogWarning("No vote question option found with ID {VoteQuestionOptionId}", request.Id);
            return Error.NotFound(description: $"No vote question option with ID {request.Id} was found");
        }

        var deleteResult = await repo.VoteQuestionOptionRepository.Remove(voteQuestionOptionResult.Value);
        if (deleteResult.IsError)
        {
            logger.LogError("Failed to delete vote question option: {Errors}", deleteResult.Errors);
            return deleteResult.Errors;
        }

        var saveResult = await repo.SaveChangesAsync(cancellationToken);
        if (saveResult.IsError)
        {
            logger.LogError("Failed to save changes after deleting vote question option: {Errors}", saveResult.Errors);
            return saveResult.Errors;
        }

        // Get admin from context to log action
        var getAdminFromContext = context.HttpContext?.User;
        string performedBy = "Unknown Admin";
        if (getAdminFromContext != null && (getAdminFromContext.IsInRole("Admin") || getAdminFromContext.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin")))
        {
            performedBy = getAdminFromContext.Identity.Name ?? "Unknown Admin";
        }

        // Log the vote question option deletion
        var systemLog = new SystemLog
        {
            Action = $"Vote question option with ID {request.Id} deleted",
            PerformedBy = performedBy,
            TimeStamp = DateTime.UtcNow
        };
        await repo.SystemLogRepository.AddAsync(systemLog);
        await repo.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully deleted vote question option with ID {VoteQuestionOptionId}", request.Id);
        return Result.Deleted;
    }
}