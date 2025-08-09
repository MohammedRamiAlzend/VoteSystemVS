using Application.Features.UserGroup.Common;
using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.UserGroup.Queries.GetById;

public class GetUserQueryHandler
    (
        IUnitOfWork repo,
        ILogger<GetUserQueryHandler> logger
    ) : IRequestHandler<GetUserQuery, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving user with ID {UserId}", request.Id);

        var userResult = await repo.UserRepository.GetByIdAsync(request.Id);
        if (userResult.IsError || userResult.Value == null)
        {
            logger.LogWarning("No user found with ID {UserId}", request.Id);
            return Error.NotFound(description: $"No user with ID {request.Id} was found");
        }

        var user = userResult.Value;
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

        logger.LogInformation("Successfully retrieved user with ID {UserId}", request.Id);
        return dto;
    }
}