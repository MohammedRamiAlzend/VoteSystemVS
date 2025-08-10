using Application.Features.Auth.Common;
using Domain.Common.Results;
using Domain.Entities;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Commands.Login.UserLogin.WithEmail;

public record UserLoginWithEmailCommand(string Email, string OTP) : IRequest<Result<string>>;

public class UserLoginWithEmailCommandHandler
    (
        IUnitOfWork repo,
        IJwtTokenService jwtTokenService
    ) : IRequestHandler<UserLoginWithEmailCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserLoginWithEmailCommand request, CancellationToken cancellationToken)
    {
        var getOtp = await repo.OTPCodeRepository.GetByFilterAsync(x => x.Code == request.OTP, include: i => i.Include(x => x.AttendanceUser).ThenInclude(x => x.VoteSession).Include(x => x.AttendanceUser).ThenInclude(x => x.User));
        if (getOtp.IsError)
        {
            return getOtp.Errors;
        }
        if (getOtp.Value is null)
        {
            return Error.NotFound();
        }
        if (!getOtp.Value.IsOTPValid())
        {
            return Error.Validation(description: "this otp is not valid");
        }
        if(getOtp.Value.IsUsed)
        {
            return Error.Validation(description: "this otp is used");
        }
        var attendanceUser = getOtp.Value.AttendanceUser;
        var voteSession = getOtp.Value.AttendanceUser.VoteSession;
        var user = getOtp.Value.AttendanceUser.User;
        var token = jwtTokenService.GenerateToken(user.Id.ToString(), "User", user.FullName, user.PhoneNumber, user.Email);
        getOtp.Value.IsUsed = true;
        var updateResult = await repo.OTPCodeRepository.UpdateAsync(getOtp.Value);
        if(updateResult.IsError)
        {
            return updateResult.Errors;
        }
        var commitResult = await repo.SaveChangesAsync(cancellationToken);
        if(commitResult.IsError)
        {
            return commitResult.Errors;
        }
        return token;
    }

    public bool IsOTPValid(OTPCode oTPCode)
    {
        if (oTPCode.ExpiredAt < DateTime.UtcNow)
            return false;
        else
            return true;
    }
}
