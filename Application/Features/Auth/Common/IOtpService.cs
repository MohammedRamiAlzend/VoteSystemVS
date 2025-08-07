namespace Application.Features.Auth.Common;

public interface IOtpService
{
    Task<string> GenerateOtpAsync(string userId);
    Task<bool> ValidateOtpAsync(string userId, string otp);
}
