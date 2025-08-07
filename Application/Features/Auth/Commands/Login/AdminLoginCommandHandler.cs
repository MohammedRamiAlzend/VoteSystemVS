using System.Threading;
using System.Threading.Tasks;
using Application.Features.Auth.Common;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Domain.Common.Results;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
namespace Application.Features.Auth.Commands.Login;

public class AdminLoginCommandHandler : IRequestHandler<AdminLoginCommand, Result<AuthResultDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IMapper _mapper;

    public AdminLoginCommandHandler(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
        _mapper = mapper;
    }

    public async Task<Result<AuthResultDto>> Handle(AdminLoginCommand request, CancellationToken cancellationToken)
    {
        var adminRepo = _unitOfWork.AdminRepository;
        var adminResult = await adminRepo.FindAsync(a => a.UserName == request.UserName);
        if (adminResult.IsSuccess is false || adminResult.Value is null || !adminResult.Value.Any())
        {
            return new List<Error> { Error.Unauthorized("Unauthorized", "Invalid username or password") };
        }
        var admin = adminResult.Value.First();
        if (admin == null || !BCrypt.Net.BCrypt.Verify(request.Password, admin.HashedPassword))
        {
            return new List<Error> { Error.Unauthorized("Unauthorized", "Invalid username or password") };
        }

        var token = _jwtTokenService.GenerateToken(admin.Id.ToString(), admin.UserName, "Admin", admin.UserName, "");
        var result = new AuthResultDto
        {
            Token = token,
            UserName = admin.UserName,
            Role = "Admin",
            FullName = admin.UserName,
            PhoneNumber = string.Empty
        };
        return Result<AuthResultDto>.Success(result);
    }
}
