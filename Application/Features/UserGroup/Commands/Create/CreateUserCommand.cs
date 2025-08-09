using MediatR;
using Domain.Common.Results;
using Application.Features.UserGroup.Common;

namespace Application.Features.UserGroup.Commands.Create;

public record CreateUserCommand(
    string FullName,
    string PhoneNumber,
    string Email,
    bool IsActive,
    int CreatedByAdminId
) : IRequest<Result<UserDto>>;