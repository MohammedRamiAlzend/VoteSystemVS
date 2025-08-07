namespace Application.Features.Auth.Common;

public class AuthResultDto
{
    public string Token { get; set; }
    public string UserName { get; set; }
    public string Role { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
}
