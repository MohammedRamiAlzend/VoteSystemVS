using MediatR;
using Domain.Common.Results;

namespace Application.Features.AttendanceUserGroup.Commands.Delete;

public record DeleteAttendanceUserCommand(int Id) : IRequest<Result<Deleted>>;