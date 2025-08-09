using MediatR;
using Domain.Common.Results;

namespace Application.Features.AttendanceUserGroup.Commands.Update;

public record UpdateAttendanceUserCommand(
    int Id,
    int UserId,
    int VoteSessionId,
    int? OTPCodeID
) : IRequest<Result<Updated>>;