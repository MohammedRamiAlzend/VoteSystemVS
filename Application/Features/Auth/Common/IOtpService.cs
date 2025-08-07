namespace Application.Features.Auth.Common;

public interface IOtpService
{
    void SendOtp(string phoneNumber);
    bool ValidateOtp(string phoneNumber, string otpCode);
}
