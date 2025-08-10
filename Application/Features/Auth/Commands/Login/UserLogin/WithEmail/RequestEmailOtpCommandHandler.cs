
using Application.Common.Models;
using Application.Features.Auth.Common;
using Application.Features.Services;
using Domain.Common.Results;
using Domain.Entities;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.Auth.Commands.Login.UserLogin.WithEmail;

public record RequestEmailOtpCommand(string Email) : IRequest<Result<Success>>;

public class RequestEmailOtpCommandHandler
    (
        IUnitOfWork repo,
        IEmailService emailService,
        ILogger<RequestEmailOtpCommandHandler> logger,
        IOtpService otpService
    )
    : IRequestHandler<RequestEmailOtpCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(RequestEmailOtpCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Requesting OTP for email: {Email}", request.Email);

        var userResult = await repo.AttendanceUserRepository
            .GetByFilterAsync(
                x => x.User.Email == request.Email,
                include: i => i.Include(x => x.User));

        if (userResult.IsError) return userResult.Errors;
        if (userResult.Value is null)
        {
            logger.LogWarning("User not found for email: {Email}", request.Email);
            return Error.NotFound("User not found");
        }

        var attendanceUser = userResult.Value;

        const int expirationMinutes = 10;
        var code = await otpService.GenerateOtpAsync();
        Result<Success> isCodeGenerated = Result.Success;
        do
        {
            isCodeGenerated = await repo.OTPCodeRepository.AnyAsync(x => x.Code == code);
            code = await otpService.GenerateOtpAsync();
        } while (isCodeGenerated.IsSuccess);
       
        
        attendanceUser.SetOtpCode(new OTPCode
        {
            Code = code,
            CreatedAt = DateTime.UtcNow,
            ExpiredAt = DateTime.UtcNow.AddMinutes(expirationMinutes),
            IsUsed = false
        });

        var updateResult = await repo.AttendanceUserRepository.UpdateAsync(attendanceUser);
        if (updateResult.IsError) return updateResult.Errors;

        var commitResult = await repo.SaveChangesAsync(cancellationToken);
        if (commitResult.IsError) return commitResult.Errors;

        logger.LogInformation("Generated OTP for user {UserId}", attendanceUser.Id);

        var emailResult = await emailService.SendOtpEmailAsync(new EmailDto
        {
            OTP = code,
            To = request.Email,
        });

        if (emailResult.IsError)
        {
            logger.LogError("Failed to send OTP email to {Email}: {Error}",
                request.Email, emailResult.Errors);
            return emailResult.Errors;
        }

        logger.LogInformation("Successfully sent OTP to {Email}", request.Email);
        return Result.Success;
    }
}
