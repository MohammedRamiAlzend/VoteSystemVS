using Application.Features.Auth.Common;
using AutoMapper;
using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using System.Security.Cryptography;
using System.Text;
namespace Application.Features.Auth.Commands.Login;

public class AdminLoginCommandHandler(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService, ISender mapper) : IRequestHandler<AdminLoginCommand, Result<AuthResultDto>>
{

  public async Task<Result<AuthResultDto>> Handle(AdminLoginCommand request, CancellationToken cancellationToken)
    {
        var adminRepo = unitOfWork.AdminRepository;
        var adminResult = await adminRepo.FindAsync(a => a.UserName == request.UserName);
        if (adminResult.IsSuccess is false || adminResult.Value is null || !adminResult.Value.Any())
        {
            return new List<Error> { Error.Unauthorized("Unauthorized", "Invalid username or password") };
        }
        var admin = adminResult.Value.First();
        string hashedPassword = HashPassword(request.Password);
        if (admin == null || hashedPassword != admin.HashedPassword)
        {
            return new List<Error> { Error.Unauthorized("Unauthorized", "Invalid username or password") };
        }

        var token = jwtTokenService.GenerateToken(admin.Id.ToString(), admin.UserName, "Admin", admin.UserName, "");
        var result = new AuthResultDto
        {
            Token = token,
            UserName = admin.UserName,
            Role = "Admin",
            FullName = admin.UserName,
            PhoneNumber = string.Empty
        };
        return result;
    }
    private static string HashPassword(string password)
    {
        using (var sha = SHA256.Create())
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
