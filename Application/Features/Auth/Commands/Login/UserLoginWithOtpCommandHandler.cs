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

public class UserLoginWithOtpCommandHandler : IRequestHandler<UserLoginWithOtpCommand, Result<AuthResultDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOtpService _otpService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IMapper _mapper;

    public UserLoginWithOtpCommandHandler(IUnitOfWork unitOfWork, IOtpService otpService, IJwtTokenService jwtTokenService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _otpService = otpService;
        _jwtTokenService = jwtTokenService;
        _mapper = mapper;
    }

    public async Task<Result<AuthResultDto>> Handle(UserLoginWithOtpCommand request, CancellationToken cancellationToken)
    {
        var userRepo = _unitOfWork.Repository<User>();
        var user = await userRepo.Query().FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber, cancellationToken);
        if (user == null || !_otpService.ValidateOtp(request.PhoneNumber, request.OtpCode))
        {
            return new List<Error> { Error.Unauthorized("Unauthorized", "Invalid phone number or OTP code") };
        }

        var token = _jwtTokenService.GenerateToken(user.Id.ToString(), user.FullName, "User", user.FullName, user.PhoneNumber);
        var result = new AuthResultDto
        {
            Token = token,
            UserName = user.FullName,
            Role = "User",
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber
        };
        return Result<AuthResultDto>.Success(result);
    }
}
