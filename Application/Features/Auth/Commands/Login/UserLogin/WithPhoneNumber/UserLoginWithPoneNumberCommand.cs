using MediatR;
using Application.Features.Auth.Common;
using Domain.Common.Results;

namespace Application.Features.Auth.Commands.Login.UserLogin.WithPhoneNumber;

public class UserLoginWithPoneNumberCommand : IRequest<Result<AuthResultDto>>
{
    public string PhoneNumber { get; set; }
    public string OtpCode { get; set; }
}
