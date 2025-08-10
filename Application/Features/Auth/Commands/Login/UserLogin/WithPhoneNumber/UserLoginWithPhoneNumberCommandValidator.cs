using FluentValidation;

namespace Application.Features.Auth.Commands.Login.UserLogin.WithPhoneNumber;

public class UserLoginWithPhoneNumberCommandValidator : AbstractValidator<UserLoginWithPoneNumberCommand>
{
    public UserLoginWithPhoneNumberCommandValidator()
    {
        RuleFor(x => x.PhoneNumber).NotEmpty();
        RuleFor(x => x.OtpCode).NotEmpty();
    }
}
