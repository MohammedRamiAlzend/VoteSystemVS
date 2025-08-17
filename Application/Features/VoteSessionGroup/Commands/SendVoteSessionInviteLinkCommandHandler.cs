using Application.Common.Settings;
using Application.Features.Services;
using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Features.VoteSessionGroup.Commands;

public record SendVoteSessionInviteLinkCommand(int VoteSessionId,string Email ) : IRequest<Result<Success>>;

public class SendVoteSessionInviteLinkCommandHandler(
    IUnitOfWork repo,
    IEmailService emailService,
    IOptions<FrontendSettings> frontendOptions,
    ILogger<SendVoteSessionInviteLinkCommandHandler> logger) : IRequestHandler<SendVoteSessionInviteLinkCommand, Result<Success>>
{
    private readonly FrontendSettings _frontendSettings = frontendOptions.Value;

    public async Task<Result<Success>> Handle(SendVoteSessionInviteLinkCommand request, CancellationToken cancellationToken)
    {
        var attendanceUserResult = await repo.AttendanceUserRepository.GetByFilterAsync(x=>x.VoteSessionId== request.VoteSessionId, include: i => i.Include(x => x.User));
        if (attendanceUserResult.Value is null)
        {
            logger.LogWarning("Vote session not found: {VoteSessionId}", request.VoteSessionId);
            return Error.NotFound("Vote session not found");
        }
        if (attendanceUserResult.IsError)
        {
            return attendanceUserResult.Errors;
        }

        var attendanceUser = attendanceUserResult.Value;
        if (attendanceUser == null)
        {
            logger.LogWarning("Attendance user not found for email: {Email} in VoteSessionId: {VoteSessionId}", request.Email, request.VoteSessionId);
            return Error.NotFound("Attendance user not found for this email in the vote session");
        }
        var token = Guid.NewGuid().ToString("N");
        var now = DateTime.UtcNow;
        var magicLinkToken = new Domain.Entities.VoteSessionMagicLinkToken
        {
            AttendanceUserId = attendanceUser.Id,
            VoteSessionId = request.VoteSessionId,
            Token = token,
            CreatedAt = now,
            ExpiredAt = now.AddMinutes(30),
            IsUsed = false
        };
        await repo.VoteSessionMagicLinkTokenRepository.AddAsync(magicLinkToken);
        await repo.SaveChangesAsync(cancellationToken);

        var link = $"{_frontendSettings.VoteSessionJoinBaseUrl}?token={token}";
        var result = await emailService.SendVoteSessionInviteEmailAsync(request: new SessionEmailInviteDto(attendanceUser.User.Email, link));
        if(result.IsError)
        {
            return result.Errors;
        }
        logger.LogInformation("Sent vote session magic link to {Email} for VoteSessionId: {VoteSessionId}", attendanceUser.User.Email, request.VoteSessionId);
        return Result.Success;
    }
}
