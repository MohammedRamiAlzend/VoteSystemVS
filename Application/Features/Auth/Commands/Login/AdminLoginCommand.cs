using MediatR;
using Application.Features.Auth.Common;
using Domain.Common.Results;

namespace Application.Features.Auth.Commands.Login;

public class AdminLoginCommand : IRequest<Result<AuthResultDto>>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}
