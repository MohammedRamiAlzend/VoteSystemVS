using Application.Features.Auth.Commands.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> AdminLogin(AdminLoginCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpPost("user-login-otp")]
    public async Task<IActionResult> UserLoginWithOtp(UserLoginWithOtpCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }
}
