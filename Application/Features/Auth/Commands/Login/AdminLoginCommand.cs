using MediatR;
using Application.Features.Auth.Common;

namespace Application.Features.Auth.Commands.Login;

public class AdminLoginCommand : IRequest<AuthResultDto>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}
