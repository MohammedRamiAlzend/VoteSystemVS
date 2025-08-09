using Application.Features.UserGroup.Common;
using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.UserGroup.Queries.GetAll;

public class GetAllUserQueryHandler
    (
        IUnitOfWork repo,
        ILogger<GetAllUserQueryHandler> logger
    ) : IRequestHandler<GetAllUserQuery, Result<IEnumerable<UserDto>>>
{
    public async Task<Result<IEnumerable<UserDto>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving all users");

        var usersResult = await repo.UserRepository.GetAllAsync();
        if (usersResult.IsError)
        {
            logger.LogError("Failed to retrieve users: {Errors}", usersResult.Errors);
            return usersResult.Errors;
        }

        var users = usersResult.Value;
        var dtos = users.Select(user => new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            CreatedByAdminId = user.CreatedByAdminId
        }).ToList();

        logger.LogInformation("Successfully retrieved {Count} users", dtos.Count);
        return dtos;
    }
}