using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Domain.Entities;

namespace Application.Features.AttendanceUserGroup.Commands.Update;

public class UpdateAttendanceUserCommandHandler
    (
        IUnitOfWork repo,
        ILogger<UpdateAttendanceUserCommandHandler> logger,
        IHttpContextAccessor context
    ) : IRequestHandler<UpdateAttendanceUserCommand, Result<Updated>>
{
    public async Task<Result<Updated>> Handle(UpdateAttendanceUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating attendance user with ID {AttendanceUserId}", request.Id);

        var attendanceUserResult = await repo.AttendanceUserRepository.GetByIdAsync(request.Id);
        if (attendanceUserResult.IsError || attendanceUserResult.Value == null)
        {
            logger.LogWarning("No attendance user found with ID {AttendanceUserId}", request.Id);
            return Error.NotFound(description: $"No attendance user with ID {request.Id} was found");
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

        var attendanceUser = attendanceUserResult.Value;
        attendanceUser.UserId = request.UserId;
        attendanceUser.VoteSessionId = request.VoteSessionId;
        attendanceUser.OTPCodeID = request.OTPCodeID;
        // CreatedByAdminId should not be updated here, as it's set on creation

        var updateResult = await repo.AttendanceUserRepository.UpdateAsync(attendanceUser);
        if (updateResult.IsError)
        {
            logger.LogError("Failed to update attendance user: {Errors}", updateResult.Errors);
            return updateResult.Errors;
        }

        var saveResult = await repo.SaveChangesAsync(cancellationToken);
        if (saveResult.IsError)
        {
            logger.LogError("Failed to save changes after updating attendance user: {Errors}", saveResult.Errors);
            return saveResult.Errors;
        }

        // Log the attendance update
        var systemLog = new SystemLog
        {
            Action = $"Attendance updated for UserId {request.UserId} in VoteSessionId {request.VoteSessionId}",
            PerformedBy = getAdminFromContext.Identity.Name ?? "Unknown Admin",
            TimeStamp = DateTime.UtcNow
        };
        await repo.SystemLogRepository.AddAsync(systemLog);
        await repo.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully updated attendance user with ID {AttendanceUserId}", request.Id);
        return Result.Updated;
    }
}