using Domain.Common.Results;
using Domain.Entities;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using FluentValidation;

namespace Application.Features.UserGroup.Commands.Create;

public class CreateUserCommandHandler
    (
        IUnitOfWork repo,
        ILogger<CreateUserCommandHandler> logger,
        IHttpContextAccessor context,
        IValidator<CreateUserCommand> validator
    ) : IRequestHandler<CreateUserCommand, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validatorResult.IsValid)
        {
            validatorResult.Errors.MapFromFluentValidationErrors();
        }

        logger.LogInformation("Creating user with FullName {FullName}", request.FullName);

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

        var user = new User
        {
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            IsActive = request.IsActive,
            CreatedByAdminId = getAdminId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        var addingResult = await repo.UserRepository.AddAsync(user);
        if (addingResult.IsError)
        {
            logger.LogError("Failed to add user: {Errors}", addingResult.Errors);
            return addingResult.Errors;
        }

        var commit = await repo.SaveChangesAsync(cancellationToken);
        if (commit.IsError)
        {
            logger.LogError("Failed to save changes: {Errors}", commit.Errors);
            return commit.Errors;
        }

        // Get admin from context to log action
        string performedBy = "Unknown Admin";
        var getAdminFromContextForLog = context.HttpContext?.User;
        if (getAdminFromContextForLog != null && (getAdminFromContextForLog.IsInRole("Admin") || getAdminFromContextForLog.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin")))
        {
            performedBy = getAdminFromContextForLog.Identity.Name ?? "Unknown Admin";
        }

        // Log the user creation
        var systemLog = new SystemLog
        {
            Action = $"User '{request.FullName}' created",
            PerformedBy = performedBy,
            TimeStamp = DateTime.UtcNow
        };
        await repo.SystemLogRepository.AddAsync(systemLog);
        await repo.SaveChangesAsync(cancellationToken);

        logger.LogInformation("User created successfully with FullName {FullName}", request.FullName);
        return new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            CreatedByAdminId = user.CreatedByAdminId
        };
    }
}