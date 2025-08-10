using Application.Features.Auth.Common;
using AutoMapper;
using Domain.Common.Results;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Repositories.Abstractions;
using MediatR;

namespace Application.Features.Auth.Commands.Login.UserLogin.WithPhoneNumber;

public class UserLoginWithPhoneNumberCommandHandler(IUnitOfWork unitOfWork, IOtpService otpService, IJwtTokenService jwtTokenService, IValidator<UserLoginWithPoneNumberCommand> validator) : IRequestHandler<UserLoginWithPoneNumberCommand, Result<AuthResultDto>>
{
    public async Task<Result<AuthResultDto>> Handle(UserLoginWithPoneNumberCommand request, CancellationToken cancellationToken)
    {
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        if (validatorResult.IsValid)
        {
            validatorResult.Errors.MapFromFluentValidationErrors();
        }
        var userRepo = unitOfWork.UserRepository;
        var userResult = await userRepo.FindAsync(u => u.PhoneNumber == request.PhoneNumber);
        if (userResult.IsError || userResult.Value is null || userResult.Value.Count() == 0)
        {
            return new List<Error> { Error.Unauthorized("Unauthorized", "Invalid phone number or OTP code") };
        }
        var user = userResult.Value.First();
        if (user == null || !await otpService.ValidateOtpAsync(request.PhoneNumber, request.OtpCode))
        {
            return new List<Error> { Error.Unauthorized("Unauthorized", "Invalid phone number or OTP code") };
        }

        var token = jwtTokenService.GenerateToken(user.Id.ToString(), user.FullName, "User", user.FullName, user.PhoneNumber);
        var result = new AuthResultDto
        {
            Token = token,
            UserName = user.FullName,
            Role = "User",
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber
        };

        // Log the successful user login
        var systemLog = new SystemLog
        {
            Action = "User Login via OTP",
            PerformedBy = user.PhoneNumber, // Or user.FullName if preferred
            TimeStamp = DateTime.UtcNow
        };
        await unitOfWork.SystemLogRepository.AddAsync(systemLog);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }
}
