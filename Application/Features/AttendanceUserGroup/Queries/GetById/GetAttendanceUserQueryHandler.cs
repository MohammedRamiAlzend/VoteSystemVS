using Application.Features.AttendanceUserGroup.Common;
using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.AttendanceUserGroup.Queries.GetById;

public class GetAttendanceUserQueryHandler
    (
        IUnitOfWork repo,
        ILogger<GetAttendanceUserQueryHandler> logger
    ) : IRequestHandler<GetAttendanceUserQuery, Result<AttendanceUserDto>>
{
    public async Task<Result<AttendanceUserDto>> Handle(GetAttendanceUserQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving attendance user with ID {AttendanceUserId}", request.Id);

        var attendanceUserResult = await repo.AttendanceUserRepository.GetByIdAsync(request.Id);
        if (attendanceUserResult.IsError || attendanceUserResult.Value == null)
        {
            logger.LogWarning("No attendance user found with ID {AttendanceUserId}", request.Id);
            return Error.NotFound(description: $"No attendance user with ID {request.Id} was found");
        }

        var attendanceUser = attendanceUserResult.Value;
        var dto = new AttendanceUserDto
        {
            Id = attendanceUser.Id,
            UserId = attendanceUser.UserId,
            VoteSessionId = attendanceUser.VoteSessionId,
            OTPCodeID = attendanceUser.OTPCodeID,
            CreatedByAdminId = attendanceUser.CreatedByAdminId
        };

        logger.LogInformation("Successfully retrieved attendance user with ID {AttendanceUserId}", request.Id);
        return dto;
    }
}