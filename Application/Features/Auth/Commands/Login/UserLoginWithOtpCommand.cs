using MediatR;
using Application.Features.Auth.Common;

namespace Application.Features.Auth.Commands.Login;

public class UserLoginWithOtpCommand : IRequest<AuthResultDto>
{
    public string PhoneNumber { get; set; }
    public string OtpCode { get; set; }
}
