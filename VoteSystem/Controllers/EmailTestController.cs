using Application.Features.Services;
using Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace VoteSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailTestController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailTestController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendTestEmail([FromBody] EmailDto emailDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _emailService.SendOtpEmailAsync(emailDto);
            return Ok("Test email sent successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error sending test email: {ex.Message}");
        }
    }
}
