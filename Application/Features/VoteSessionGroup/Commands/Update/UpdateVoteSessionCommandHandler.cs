using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Features.VoteSessionGroup.Commands.Update;

public class UpdateVoteSessionCommandHandler
    (
        IUnitOfWork repo,
        ILogger<UpdateVoteSessionCommandHandler> logger,
        IValidator<UpdateVoteSessionCommand> validator,
        IHttpContextAccessor context
    ) : IRequestHandler<UpdateVoteSessionCommand, Result<Updated>>
{
    public async Task<Result<Updated>> Handle(UpdateVoteSessionCommand request, CancellationToken cancellationToken)
    {
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validatorResult.IsValid)
        {
            validatorResult.Errors.MapFromFluentValidationErrors();
        }

        logger.LogInformation("Updating vote session with ID {VoteSessionId}", request.Id);

        var voteSessionResult = await repo.VoteSessionRepository.GetByIdAsync(request.Id);
        if (voteSessionResult.IsError || voteSessionResult.Value == null)
        {
            logger.LogWarning("No vote session found with ID {VoteSessionId}", request.Id);
            return Error.NotFound(description: $"No vote session with ID {request.Id} was found");
        }

        var getAdminFromContext = context.HttpContext?.User;
        if (getAdminFromContext == null)
        {
            logger.LogWarning("No admin context found in HTTP context");
            return Error.Unauthorized();
        }

        if ((getAdminFromContext.IsInRole("Admin") || getAdminFromContext.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin")) is false)
        {
            logger.LogWarning("User is not authorized as admin");
            return Error.Forbidden();
        }

        var adminIdClaim = getAdminFromContext.FindFirst(ClaimTypes.NameIdentifier);
        if (adminIdClaim == null || !int.TryParse(adminIdClaim.Value, out var getAdminId))
        {
            logger.LogWarning("Invalid or missing admin ID claim");
            return Error.Unauthorized(description: "Invalid or missing admin ID claim.");
        }

        var isAdminExist = await repo.AdminRepository.AnyAsync(x => x.Id == getAdminId);
        if (isAdminExist.IsError)
        {
            logger.LogWarning("No admin found with ID {AdminId}", getAdminId);
            return Error.NotFound(description: $"no admin with {getAdminId} was found ");
        }

        var voteSession = voteSessionResult.Value;
        voteSession.Description = request.Description;
        voteSession.StartedAt = request.StartedAt;
        voteSession.TopicTitle = request.TopicTitle;
        voteSession.EndedAt = request.EndedAt;

        var updateResult = await repo.VoteSessionRepository.UpdateAsync(voteSession);
        if (updateResult.IsError)
        {
            logger.LogError("Failed to update vote session: {Errors}", updateResult.Errors);
            return updateResult.Errors;
        }

        var saveResult = await repo.SaveChangesAsync(cancellationToken);
        if (saveResult.IsError)
        {
            logger.LogError("Failed to save changes after updating vote session: {Errors}", saveResult.Errors);
            return saveResult.Errors;
        }

        // Log the vote session update
        var systemLog = new SystemLog
        {
            Action = $"Vote session with ID {request.Id} updated",
            PerformedBy = getAdminFromContext.Identity.Name ?? "Unknown Admin",
            TimeStamp = DateTime.UtcNow
        };
        await repo.SystemLogRepository.AddAsync(systemLog);
        await repo.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully updated vote session with ID {VoteSessionId}", request.Id);
        return Result.Updated;
    }
}