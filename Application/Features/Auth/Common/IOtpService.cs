namespace Application.Features.Auth.Common;

public interface IOtpService
{
    Task<string> GenerateOtpAsync();
    Task<bool> ValidateOtpAsync(string userId, string otp);
}
