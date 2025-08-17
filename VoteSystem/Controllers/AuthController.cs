using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Login.UserLogin.WithEmail;
using Application.Features.Auth.Commands.Login.UserLogin.WithPhoneNumber;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace VoteSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("admin-login")]
    public async Task<IActionResult> AdminLogin(AdminLoginCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpPost("user-login-phoneNumber")]
    public async Task<IActionResult> UserLoginWithPhoneNumber(UserLoginWithPoneNumberCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }
    [HttpPost("user-login-Email")]
    public async Task<IActionResult> UserLoginWithEmail(UserLoginWithEmailCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }
    [HttpPost("request-email-otp")]
    public async Task<IActionResult> RequestEmailOtp(RequestEmailOtpCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    // Magic link login endpoint
    [HttpPost("user-login-magic-link")]
    public async Task<IActionResult> UserLoginWithMagicLink([FromBody] UserLoginWithMagicLinkCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }
}
