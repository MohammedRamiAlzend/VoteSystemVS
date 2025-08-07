using FluentValidation;

namespace Application.Features.Auth.Commands.Login;

public class UserLoginWithOtpCommandValidator : AbstractValidator<UserLoginWithOtpCommand>
{
    public UserLoginWithOtpCommandValidator()
    {
        RuleFor(x => x.PhoneNumber).NotEmpty();
        RuleFor(x => x.OtpCode).NotEmpty();
    }
}
