using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Domain.Entities;

namespace Application.Features.AttendanceUserGroup.Commands.Delete;

public class DeleteAttendanceUserCommandHandler
    (
        IUnitOfWork repo,
        ILogger<DeleteAttendanceUserCommandHandler> logger,
        IHttpContextAccessor context
    ) : IRequestHandler<DeleteAttendanceUserCommand, Result<Deleted>>
{
    public async Task<Result<Deleted>> Handle(DeleteAttendanceUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting attendance user with ID {AttendanceUserId}", request.Id);

        var attendanceUserResult = await repo.AttendanceUserRepository.GetByIdAsync(request.Id);
        if (attendanceUserResult.IsError || attendanceUserResult.Value == null)
        {
            logger.LogWarning("No attendance user found with ID {AttendanceUserId}", request.Id);
            return Error.NotFound(description: $"No attendance user with ID {request.Id} was found");
        }

        var deleteResult = await repo.AttendanceUserRepository.Remove(attendanceUserResult.Value);
        if (deleteResult.IsError)
        {
            logger.LogError("Failed to delete attendance user: {Errors}", deleteResult.Errors);
            return deleteResult.Errors;
        }

        var saveResult = await repo.SaveChangesAsync(cancellationToken);
        if (saveResult.IsError)
        {
            logger.LogError("Failed to save changes after deleting attendance user: {Errors}", saveResult.Errors);
            return saveResult.Errors;
        }

        // Get admin from context to log action
        var getAdminFromContext = context.HttpContext?.User;
        string performedBy = "Unknown Admin";
        if (getAdminFromContext != null && (getAdminFromContext.IsInRole("Admin") || getAdminFromContext.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin")))
        {
            performedBy = getAdminFromContext.Identity.Name ?? "Unknown Admin";
        }

        // Log the attendance deletion
        var systemLog = new SystemLog
        {
            Action = $"Attendance user with ID {request.Id} deleted",
            PerformedBy = performedBy,
            TimeStamp = DateTime.UtcNow
        };
        await repo.SystemLogRepository.AddAsync(systemLog);
        await repo.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Successfully deleted attendance user with ID {AttendanceUserId}", request.Id);
        return Result.Deleted;
    }
}