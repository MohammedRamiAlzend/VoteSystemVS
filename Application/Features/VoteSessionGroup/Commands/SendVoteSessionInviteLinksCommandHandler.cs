using Application.Common.Settings;
using Application.Features.Services;
using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Features.VoteSessionGroup.Commands;

public record SendVoteSessionInviteLinksCommand(int VoteSessionId) : IRequest<Result<Success>>;

public partial class SendVoteSessionInviteLinksCommandHandler(
    IUnitOfWork repo,
    IEmailService emailService,
    IOptions<FrontendSettings> frontendOptions,
    ILogger<SendVoteSessionInviteLinksCommandHandler> logger) : IRequestHandler<SendVoteSessionInviteLinksCommand, Result<Success>>
{
    private readonly FrontendSettings _frontendSettings = frontendOptions.Value;

    public async Task<Result<Success>> Handle(SendVoteSessionInviteLinksCommand request, CancellationToken cancellationToken)
    {
        var attendanceUsersResult = await repo.AttendanceUserRepository.GetAllAsync(x=>x.VoteSessionId == request.VoteSessionId,include: i=>i.Include(x=>x.User));
        if (attendanceUsersResult.IsError) { return attendanceUsersResult.Errors; }
        if (attendanceUsersResult.Value is null)
        {
            logger.LogWarning("Vote session not found: {VoteSessionId}", request.VoteSessionId);
            return Error.NotFound("Vote session not found");
        }
        var attendanceUsers = attendanceUsersResult.Value;
        foreach (var attendanceUser in attendanceUsers)
        {
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
            var link = $"{_frontendSettings.VoteSessionJoinBaseUrl}?token={token}";
            await emailService.SendVoteSessionInviteEmailAsync(new SessionEmailInviteDto(attendanceUser.User.Email, link));
        }
        await repo.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Sent vote session magic links for VoteSessionId: {VoteSessionId}", request.VoteSessionId);
        return Result.Success;
    }
}
