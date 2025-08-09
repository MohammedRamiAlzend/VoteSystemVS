using Domain.Common.Results;
using FluentValidation;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Domain.Entities;

namespace Application.Features.VoteSessionGroup.Commands.UpdateVoteSessionStatus;

public class UpdateVoteSessionStatusCommandHandler(
    IUnitOfWork repo,
    ILogger<UpdateVoteSessionStatusCommandHandler> logger,
    IValidator<UpdateVoteSessionStatusCommand> validator,
    IHttpContextAccessor context)
        : IRequestHandler<UpdateVoteSessionStatusCommand, Result<Updated>>
{
    public async Task<Result<Updated>> Handle(UpdateVoteSessionStatusCommand request, CancellationToken cancellationToken)
    {
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        if (validatorResult.IsValid)
        {
            validatorResult.Errors.MapFromFluentValidationErrors();
        }
        logger.LogInformation("Processing UpdateVoteSessionStatus for VoteSessionId {VoteSessionId}", request.VoteSessionId);

        var getVoteSession = await repo.VoteSessionRepository.GetByIdAsync(request.VoteSessionId);
        if (getVoteSession.IsError)
        {
            logger.LogWarning("Failed to retrieve VoteSession with ID {VoteSessionId}: {Errors}", request.VoteSessionId, getVoteSession.Errors);
            return getVoteSession.Errors;
        }
        if (getVoteSession.Value is null)
        {
            logger.LogWarning("VoteSession with ID {VoteSessionId} not found", request.VoteSessionId);
            return Error.NotFound(description: "Vote session does not exist");
        }
        if (getVoteSession.Value.VoteSessionStatus is Domain.Entities.VoteSessionStatus.Closed)
        {
            return Error.Failure(description: "you cannot update closed vote session status");
        }
        getVoteSession.Value.VoteSessionStatus = request.VoteSessionStatus;
        var updateResult = await repo.VoteSessionRepository.UpdateAsync(getVoteSession.Value);
        if (updateResult.IsError)
        {
            logger.LogError("Failed to update VoteSession with ID {VoteSessionId}: {Errors}", request.VoteSessionId, updateResult.Errors);
            return updateResult.Errors;
        }
        var saveResult = await repo.SaveChangesAsync(cancellationToken);
        if (saveResult.IsError)
        {
            logger.LogError("Failed to save changes for VoteSessionId {VoteSessionId}: {Errors}", request.VoteSessionId, saveResult.Errors);
            return saveResult.Errors;
        }

        // Get admin from context to log action
        var getAdminFromContext = context.HttpContext?.User;
        string performedBy = "Unknown Admin";
        if (getAdminFromContext != null && (getAdminFromContext.IsInRole("Admin") || getAdminFromContext.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin")))
        {
            performedBy = getAdminFromContext.Identity.Name ?? "Unknown Admin";
        }

        // Log the vote session status update
        var systemLog = new SystemLog
        {
            Action = $"Vote session status updated for VoteSessionId {request.VoteSessionId} to {request.VoteSessionStatus}",
            PerformedBy = performedBy,
            TimeStamp = DateTime.UtcNow
        };
        await repo.SystemLogRepository.AddAsync(systemLog);
        await repo.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully updated VoteSessionStatus for VoteSessionId {VoteSessionId} to {Status}", request.VoteSessionId, request.VoteSessionStatus);

        return Result.Updated;
    }
}