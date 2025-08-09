using MediatR;
using Domain.Common.Results;
using Application.Features.AttendanceUserGroup.Common;

namespace Application.Features.AttendanceUserGroup.Queries.GetAll;

public record GetAllAttendanceUserQuery() : IRequest<Result<IEnumerable<AttendanceUserDto>>>;