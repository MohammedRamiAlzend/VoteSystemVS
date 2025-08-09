using Domain.Common.Results;
using Domain.Entities;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Application.Features.AttendanceUserGroup.Commands.Create
{
    public class CreateUserAttendanceCommandHandler
        (
            IUnitOfWork repo,
            ILogger<CreateUserAttendanceCommandHandler> logger,
            IHttpContextAccessor context
        )
        : IRequestHandler<CreateUserAttencanceCommand, Result<Created>>
    {
        public async Task<Result<Created>> Handle(CreateUserAttencanceCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting attendance creation for UserId {UserId} in VoteSessionId {VoteSessionId}", request.UserId, request.VoteSessionId);

            var getVoteSession = await repo.VoteSessionRepository.AnyAsync(x => x.Id == request.VoteSessionId);
            if (getVoteSession.IsError)
            {
                logger.LogWarning("No vote session found with ID {VoteSessionId}", request.VoteSessionId);
                return Error.NotFound(description: $"no vote session with {request.VoteSessionId} was found");
            }

            var getUser = await repo.UserRepository.AnyAsync(x => x.Id == request.UserId);
            if (getUser.IsError)
            {
                logger.LogWarning("No user found with ID {UserId}", request.UserId);
                return Error.NotFound(description: $"no user with {request.UserId} was found");
            }

            var getAdminFromContext = context.HttpContext?.User;
            if (getAdminFromContext == null)
            {
                logger.LogWarning("No admin context found in HTTP context");
                return Error.Unauthorized();
            }

            if ((getAdminFromContext.IsInRole("Admin") || getAdminFromContext.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin")) is false)
            {
                logger.LogWarning("User is not authorized as admin");
                return Error.Forbidden();
            }

            var adminIdClaim = getAdminFromContext.FindFirst(ClaimTypes.NameIdentifier);
            if (adminIdClaim == null || !int.TryParse(adminIdClaim.Value, out var getAdminId))
            {
                logger.LogWarning("Invalid or missing admin ID claim");
                return Error.Unauthorized(description: "Invalid or missing admin ID claim.");
            }

            var isAdminExist = await repo.AdminRepository.AnyAsync(x => x.Id == getAdminId);
            if (isAdminExist.IsError)
            {
                logger.LogWarning("No admin found with ID {AdminId}", getAdminId);
                return Error.NotFound(description: $"no admin with {getAdminId} was found ");
            }

            var userAttendance = new AttendanceUser
            {
                VoteSessionId = request.VoteSessionId,
                OTPCodeID = null,
                UserId = request.UserId,
                CreatedByAdminId = getAdminId,
            };

            var addingResult = await repo.AttendanceUserRepository.AddAsync(userAttendance);
            if (addingResult.IsError)
            {
                logger.LogError("Failed to add attendance: {Errors}", addingResult.Errors);
                return addingResult.Errors;
            }

            var commit = await repo.SaveChangesAsync(cancellationToken);
            if (commit.IsError)
            {
                logger.LogError("Failed to save changes: {Errors}", commit.Errors);
                return commit.Errors;
            }

            logger.LogInformation("Attendance created successfully for UserId {UserId} in VoteSessionId {VoteSessionId}", request.UserId, request.VoteSessionId);
            return Result.Created;
        }
    }
}