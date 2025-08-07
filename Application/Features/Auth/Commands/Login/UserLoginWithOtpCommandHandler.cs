using System.Threading;
using System.Threading.Tasks;
using Application.Features.Auth.Common;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Domain.Common.Results;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Auth.Commands.Login;

public class UserLoginWithOtpCommandHandler(IUnitOfWork unitOfWork, IOtpService otpService, IJwtTokenService jwtTokenService, ISender mapper) : IRequestHandler<UserLoginWithOtpCommand, Result<AuthResultDto>>
{
    public async Task<Result<AuthResultDto>> Handle(UserLoginWithOtpCommand request, CancellationToken cancellationToken)
    {
        var userRepo = unitOfWork.UserRepository;
        var userResult = await userRepo.FindAsync(u => u.PhoneNumber == request.PhoneNumber);
        if (userResult.IsError || userResult.Value is null || userResult.Value.Count() == 0)
        {
            return new List<Error> { Error.Unauthorized("Unauthorized", "Invalid phone number or OTP code") };
        }
        var user = userResult.Value.First();
        if (user == null || !(await otpService.ValidateOtpAsync(request.PhoneNumber, request.OtpCode)))
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
        return result;
    }
}
