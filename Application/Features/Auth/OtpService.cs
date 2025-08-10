using Application.Features.Auth.Common;

namespace Application.Features.Auth;

public class OtpService : IOtpService
{
    public Task<string> GenerateOtpAsync()
    {
        Random random = new();
        string otp = random.Next(100000, 999999).ToString();
        return Task.FromResult(otp);
    }

    public Task<bool> ValidateOtpAsync(string userId, string otp)
    {
        // In a real application, you would retrieve the stored OTP for the userId
        // and compare it with the provided OTP, also checking for expiry.
        // For this example, we'll just assume it's valid if it's not empty.

        return Task.FromResult(!string.IsNullOrEmpty(otp));
    }
}
