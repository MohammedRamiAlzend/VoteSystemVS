using MediatR;
using Domain.Common.Results;
using Application.Features.UserGroup.Common;

namespace Application.Features.UserGroup.Queries.GetAll;

public record GetAllUserQuery() : IRequest<Result<IEnumerable<UserDto>>>;