using MediatR;
using Domain.Common.Results;
using Application.Features.UserGroup.Common;

namespace Application.Features.UserGroup.Queries.GetById;

public record GetUserQuery(int Id) : IRequest<Result<UserDto>>;