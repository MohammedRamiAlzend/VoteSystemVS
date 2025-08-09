using MediatR;
using Domain.Common.Results;
using Application.Features.UserGroup.Common;

namespace Application.Features.UserGroup.Queries.GetByPhoneNumberOrEmail;

public record GetUserByPhoneNumberOrEmailQuery(
    string? PhoneNumber,
    string? Email
) : IRequest<Result<UserDto>>;