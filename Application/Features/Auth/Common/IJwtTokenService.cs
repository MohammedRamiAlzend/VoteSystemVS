namespace Application.Features.Auth.Common;

public interface IJwtTokenService
{
    string GenerateToken(string userId, string role, string? fullName = null, string? phoneNumber = null,string? email = null);
}
