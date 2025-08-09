using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using FluentValidation;
using Domain.Entities;

namespace Application.Features.UserGroup.Commands.Update;

public class UpdateUserCommandHandler
    (
        IUnitOfWork repo,
        ILogger<UpdateUserCommandHandler> logger,
        IHttpContextAccessor context,
        IValidator<UpdateUserCommand> validator
    ) : IRequestHandler<UpdateUserCommand, Result<Updated>>
{
    public async Task<Result<Updated>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validatorResult.IsValid)
        {
            validatorResult.Errors.MapFromFluentValidationErrors();
        }

        logger.LogInformation("Updating user with ID {UserId}", request.Id);

        var userResult = await repo.UserRepository.GetByIdAsync(request.Id);
        if (userResult.IsError || userResult.Value == null)
        {
            logger.LogWarning("No user found with ID {UserId}", request.Id);
            return Error.NotFound(description: $"No user with ID {request.Id} was found");
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

        var user = userResult.Value;
        user.FullName = request.FullName;
        user.PhoneNumber = request.PhoneNumber;
        user.Email = request.Email;
        user.IsActive = request.IsActive;

        var updateResult = await repo.UserRepository.UpdateAsync(user);
        if (updateResult.IsError)
        {
            logger.LogError("Failed to update user: {Errors}", updateResult.Errors);
            return updateResult.Errors;
        }

        var saveResult = await repo.SaveChangesAsync(cancellationToken);
        if (saveResult.IsError)
        {
            logger.LogError("Failed to save changes after updating user: {Errors}", saveResult.Errors);
            return saveResult.Errors;
        }

        // Get admin from context to log action
        string performedBy = "Unknown Admin";
        var getAdminFromContextForLog = context.HttpContext?.User;
        if (getAdminFromContextForLog != null && (getAdminFromContextForLog.IsInRole("Admin") || getAdminFromContextForLog.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin")))
        {
            performedBy = getAdminFromContextForLog.Identity.Name ?? "Unknown Admin";
        }

        // Log the user update
        var systemLog = new SystemLog
        {
            Action = $"User with ID {request.Id} updated",
            PerformedBy = performedBy,
            TimeStamp = DateTime.UtcNow
        };
        await repo.SystemLogRepository.AddAsync(systemLog);
        await repo.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully updated user with ID {UserId}", request.Id);
        return Result.Updated;
    }
}