namespace Application.Features.Auth.Common;

public interface IJwtTokenService
{
    string GenerateToken(string userId, string userName, string role, string fullName, string phoneNumber);
}
