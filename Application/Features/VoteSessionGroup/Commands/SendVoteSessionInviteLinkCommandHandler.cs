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
        var link = $"{_frontendSettings.VoteSessionJoinBaseUrl}?sessionId={request.VoteSessionId}&email={attendanceUser.User.Email}";
        
        var result = await emailService.SendVoteSessionInviteEmailAsync(request: new SessionEmailInviteDto(attendanceUser.User.Email, link));
        if(result.IsError)
        {
            return result.Errors;
        }
        logger.LogInformation("Sent vote session invite link to {Email} for VoteSessionId: {VoteSessionId}", attendanceUser.User.Email, voteSession.Id);
        return Result.Success;
    }
}
