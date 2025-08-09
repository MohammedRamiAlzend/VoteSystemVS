using Application.Features.AttendanceUserGroup.Common;
using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.AttendanceUserGroup.Queries.GetAll;

public class GetAllAttendanceUserQueryHandler
    (
        IUnitOfWork repo,
        ILogger<GetAllAttendanceUserQueryHandler> logger
    ) : IRequestHandler<GetAllAttendanceUserQuery, Result<IEnumerable<AttendanceUserDto>>>
{
    public async Task<Result<IEnumerable<AttendanceUserDto>>> Handle(GetAllAttendanceUserQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving all attendance users");

        var attendanceUsersResult = await repo.AttendanceUserRepository.GetAllAsync();
        if (attendanceUsersResult.IsError)
        {
            logger.LogError("Failed to retrieve attendance users: {Errors}", attendanceUsersResult.Errors);
            return attendanceUsersResult.Errors;
        }

        var attendanceUsers = attendanceUsersResult.Value;
        var dtos = attendanceUsers.Select(attendanceUser => new AttendanceUserDto
        {
            Id = attendanceUser.Id,
            UserId = attendanceUser.UserId,
            VoteSessionId = attendanceUser.VoteSessionId,
            OTPCodeID = attendanceUser.OTPCodeID,
            CreatedByAdminId = attendanceUser.CreatedByAdminId
        }).ToList();

        logger.LogInformation("Successfully retrieved {Count} attendance users", dtos.Count);
        return dtos;
    }
}