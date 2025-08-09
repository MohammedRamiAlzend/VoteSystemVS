using Application.Features.Auth.Commands.Login;
using Application.Features.VoteSessionGroup.Commands.Create;
using Application.Features.VoteSessionGroup.Common;
using Domain.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace VoteSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("admin-login")]
    public async Task<IActionResult> AdminLogin([FromBody] AdminLoginCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("user-login-otp")]
    public async Task<IActionResult> UserLoginWithOtp([FromBody] UserLoginWithOtpCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
