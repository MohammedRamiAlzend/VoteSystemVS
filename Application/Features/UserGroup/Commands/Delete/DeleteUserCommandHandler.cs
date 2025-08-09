using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Domain.Entities;

namespace Application.Features.UserGroup.Commands.Delete;

public class DeleteUserCommandHandler
    (
        IUnitOfWork repo,
        ILogger<DeleteUserCommandHandler> logger,
        IHttpContextAccessor context
    ) : IRequestHandler<DeleteUserCommand, Result<Deleted>>
{
    public async Task<Result<Deleted>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting user with ID {UserId}", request.Id);

        var userResult = await repo.UserRepository.GetByIdAsync(request.Id);
        if (userResult.IsError || userResult.Value == null)
        {
            logger.LogWarning("No user found with ID {UserId}", request.Id);
            return Error.NotFound(description: $"No user with ID {request.Id} was found");
        }

        var deleteResult = await repo.UserRepository.Remove(userResult.Value);
        if (deleteResult.IsError)
        {
            logger.LogError("Failed to delete user: {Errors}", deleteResult.Errors);
            return deleteResult.Errors;
        }

        var saveResult = await repo.SaveChangesAsync(cancellationToken);
        if (saveResult.IsError)
        {
            logger.LogError("Failed to save changes after deleting user: {Errors}", saveResult.Errors);
            return saveResult.Errors;
        }

        // Get admin from context to log action
        string performedBy = "Unknown Admin";
        var getAdminFromContext = context.HttpContext?.User;
        if (getAdminFromContext != null && (getAdminFromContext.IsInRole("Admin") || getAdminFromContext.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin")))
        {
            performedBy = getAdminFromContext.Identity.Name ?? "Unknown Admin";
        }

        // Log the user deletion
        var systemLog = new SystemLog
        {
            Action = $"User with ID {request.Id} deleted",
            PerformedBy = performedBy,
            TimeStamp = DateTime.UtcNow
        };
        await repo.SystemLogRepository.AddAsync(systemLog);
        await repo.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully deleted user with ID {UserId}", request.Id);
        return Result.Deleted;
    }
}