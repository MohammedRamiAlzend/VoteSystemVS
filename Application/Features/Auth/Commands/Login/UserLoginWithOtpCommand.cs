using MediatR;
using Application.Features.Auth.Common;
using Domain.Common.Results;

namespace Application.Features.Auth.Commands.Login;

public class UserLoginWithOtpCommand : IRequest<Result<AuthResultDto>>
{
    public string PhoneNumber { get; set; }
    public string OtpCode { get; set; }
}
