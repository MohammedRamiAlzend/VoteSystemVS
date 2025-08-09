using MediatR;
using Domain.Common.Results;

namespace Application.Features.UserGroup.Commands.Update;

public record UpdateUserCommand(
    int Id,
    string FullName,
    string PhoneNumber,
    string Email,
    bool IsActive
) : IRequest<Result<Updated>>;