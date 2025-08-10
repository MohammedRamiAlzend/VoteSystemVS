using Application.Features.Auth.Common;
using AutoMapper;
using Domain.Common.Results;
using FluentValidation;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using System.Security.Cryptography;
using System.Text;
using Domain.Entities;

namespace Application.Features.Auth.Commands.Login;

public class AdminLoginCommandHandler(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService, IValidator<AdminLoginCommand> validator) : IRequestHandler<AdminLoginCommand, Result<AuthResultDto>>
{

    public async Task<Result<AuthResultDto>> Handle(AdminLoginCommand request, CancellationToken cancellationToken)
    {
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        if (validatorResult.IsValid)
        {
            validatorResult.Errors.MapFromFluentValidationErrors();
        }
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

        var token = jwtTokenService.GenerateToken(admin.Id.ToString(), "Admin");
        var result = new AuthResultDto
        {
            Token = token,
            UserName = admin.UserName,
            Role = "Admin",
            FullName = admin.UserName,
            PhoneNumber = string.Empty
        };

        // Log the successful admin login
        var systemLog = new SystemLog
        {
            Action = "Admin Login",
            PerformedBy = admin.UserName,
            TimeStamp = DateTime.UtcNow
        };
        await unitOfWork.SystemLogRepository.AddAsync(systemLog);
        await unitOfWork.SaveChangesAsync(cancellationToken);

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
