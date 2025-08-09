using MediatR;
using Domain.Common.Results;

namespace Application.Features.UserGroup.Commands.Delete;

public record DeleteUserCommand(int Id) : IRequest<Result<Deleted>>;