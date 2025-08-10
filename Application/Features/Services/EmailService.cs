using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Microsoft.Extensions.Logging;
using Application.Common.Models;
using Domain.Common.Results;
using Application.Features.VoteSessionGroup.Commands;

namespace Application.Features.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration config, ILogger<EmailService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task<Result<Success>> SendOtpEmailAsync(EmailDto request)
    {
        var email = new MimeMessage();
        var emailUsername = _config.GetSection("EmailUsername").Value;
        var emailHost = _config.GetSection("EmailHost").Value;
        var emailPassword = _config.GetSection("EmailPassword").Value;

        email.From.Add(MailboxAddress.Parse(emailUsername));
        email.To.Add(MailboxAddress.Parse(request.To));
        email.Subject = "Otp Verfication";
        email.Body = new TextPart(TextFormat.Html)
        {
            Text = GetOtpEmailTemplate(request.OTP),
        };

        using var smtp = new SmtpClient();
        try
        {
            await smtp.ConnectAsync(emailHost, 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(emailUsername, emailPassword);
            await smtp.SendAsync(email);
            return Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error sending email: {ex.Message}");
            return Error.Failure(description: $"Error sending email: {ex.Message}");
        }
        finally
        {
            await smtp.DisconnectAsync(true);
        }
    }
    public static string GetOtpEmailTemplate(string otp)
    {
        return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>Email Verification</title>
                    <style>
                        body {{
                            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                            line-height: 1.6;
                            color: #333333;
                            margin: 0;
                            padding: 0;
                            background-color: #f5f5f5;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 20px auto;
                            padding: 20px;
                            background-color: #ffffff;
                            border-radius: 8px;
                            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                        }}
                        .header {{
                            text-align: center;
                            padding: 20px 0;
                            border-bottom: 1px solid #eeeeee;
                        }}
                        .content {{
                            padding: 20px;
                        }}
                        .otp-container {{
                            margin: 30px 0;
                            text-align: center;
                        }}
                        .otp-code {{
                            display: inline-block;
                            padding: 15px 30px;
                            font-size: 24px;
                            font-weight: bold;
                            letter-spacing: 5px;
                            color: #ffffff;
                            background-color: #4a6bff;
                            border-radius: 5px;
                            text-decoration: none;
                        }}
                        .footer {{
                            text-align: center;
                            padding: 20px;
                            font-size: 12px;
                            color: #999999;
                            border-top: 1px solid #eeeeee;
                        }}
                        .note {{
                            font-size: 14px;
                            color: #666666;
                            margin-top: 30px;
                            padding: 10px;
                            background-color: #f9f9f9;
                            border-radius: 4px;
                        }}
                        h2 {{
                            color: #333333;
                            margin-top: 0;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='content'>
                            <h2>Verify Your Email Address</h2>
                            <p>Hello,</p>
                            <p>Thank you for registering with us. To complete your registration, please enter the following One-Time Password (OTP) to verify your email address:</p>
                            
                            <div class='otp-container'>
                                <div class='otp-code'>{otp}</div>
                            </div>
                            
                            <p>This code will expire in 10 minutes. If you didn't request this code, you can safely ignore this email.</p>
                            
                            <div class='note'>
                                <p><strong>Note:</strong> For security reasons, please do not share this OTP with anyone.</p>
                            </div>
                        </div>
                        
                        <div class='footer'>
                            <p>© {DateTime.Now.Year} Your Company Name. All rights reserved.</p>
                            <p>If you have any questions, please contact us at support@example.com</p>
                        </div>
                    </div>
                </body>
                </html>";
    }

    public async Task<Result<Success>> SendVoteSessionInviteEmailAsync(SessionEmailInviteDto request)
    {
        using var smtp = new SmtpClient();
        try
        {
            var email = new MimeMessage();
            var emailUsername = _config.GetSection("EmailUsername").Value;
            var emailHost = _config.GetSection("EmailHost").Value;
            var emailPassword = _config.GetSection("EmailPassword").Value;

            email.From.Add(MailboxAddress.Parse(emailUsername));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = "Invitation to Vote Session";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = GetEmailInviteTemplate(request.Link)
            };

            await smtp.ConnectAsync(emailHost, 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(emailUsername, emailPassword);
            await smtp.SendAsync(email);
            return Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Error sending email: {ex.Message}");
            return Error.Failure(description: $"Error sending email: {ex.Message}");
        }
        finally
        {
            await smtp.DisconnectAsync(true);
        }
    }

    public string GetEmailInviteTemplate(string voteLink)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            color: #333;
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
        }}
        .container {{
            border: 1px solid #e0e0e0;
            border-radius: 5px;
            padding: 20px;
        }}
        .button {{
            display: inline-block;
            padding: 10px 20px;
            background-color: #4CAF50;
            color: white;
            text-decoration: none;
            border-radius: 4px;
            margin: 15px 0;
        }}
        .footer {{
            margin-top: 20px;
            font-size: 12px;
            color: #777;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <h2>Invitation to Vote Session</h2>
        <p>You have been invited to participate in a voting session. Please click the button below to access the voting page:</p>
        
        <a href=""{voteLink}"" class=""button"">Go to Voting Session</a>
        
        <p>If the button doesn't work, you can copy and paste this link into your browser:</p>
        <p><a href=""{voteLink}"">{voteLink}</a></p>
        
        <div class=""footer"">
            <p>This is an automated message. Please do not reply directly to this email.</p>
        </div>
    </div>
</body>
</html>";
    }
}
