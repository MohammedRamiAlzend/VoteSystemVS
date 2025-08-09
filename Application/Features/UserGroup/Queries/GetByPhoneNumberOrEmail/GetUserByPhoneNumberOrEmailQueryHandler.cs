using Application.Features.UserGroup.Common;
using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.UserGroup.Queries.GetByPhoneNumberOrEmail;

public class GetUserByPhoneNumberOrEmailQueryHandler
    (
        IUnitOfWork repo,
        ILogger<GetUserByPhoneNumberOrEmailQueryHandler> logger
    ) : IRequestHandler<GetUserByPhoneNumberOrEmailQuery, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(GetUserByPhoneNumberOrEmailQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving user by PhoneNumber {PhoneNumber} or Email {Email}", request.PhoneNumber, request.Email);

        if (string.IsNullOrEmpty(request.PhoneNumber) && string.IsNullOrEmpty(request.Email))
        {
            return Error.Validation(description: "Either PhoneNumber or Email must be provided.");
        }

        var userResult = new Result<IEnumerable<Domain.Entities.User>>([]);

        if (!string.IsNullOrEmpty(request.PhoneNumber))
        {
            userResult = await repo.UserRepository.FindAsync(u => u.PhoneNumber == request.PhoneNumber);
        }
        else if (!string.IsNullOrEmpty(request.Email))
        {
            userResult = await repo.UserRepository.FindAsync(u => u.Email == request.Email);
        }

        if (userResult.IsError || userResult.Value == null || !userResult.Value.Any())
        {
            logger.LogWarning("No user found with PhoneNumber {PhoneNumber} or Email {Email}", request.PhoneNumber, request.Email);
            return Error.NotFound(description: $"No user found with provided PhoneNumber or Email");
        }

        var user = userResult.Value.First();
        var dto = new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            CreatedByAdminId = user.CreatedByAdminId
        };

        logger.LogInformation("Successfully retrieved user by PhoneNumber {PhoneNumber} or Email {Email}", request.PhoneNumber, request.Email);
        return dto;
    }
}