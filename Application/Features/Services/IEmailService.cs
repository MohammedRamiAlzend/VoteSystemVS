using Application.Common.Models;
using Application.Features.VoteSessionGroup.Commands;
using Domain.Common.Results;

namespace Application.Features.Services;

public interface IEmailService
{
    Task<Result<Success>> SendOtpEmailAsync(EmailDto request);
    Task<Result<Success>> SendVoteSessionInviteEmailAsync(SessionEmailInviteDto request);
}
