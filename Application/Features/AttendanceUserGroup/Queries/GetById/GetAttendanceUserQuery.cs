using MediatR;
using Domain.Common.Results;
using Application.Features.AttendanceUserGroup.Common;

namespace Application.Features.AttendanceUserGroup.Queries.GetById;

public record GetAttendanceUserQuery(int Id) : IRequest<Result<AttendanceUserDto>>;