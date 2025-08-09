using FluentValidation;

namespace Application.Features.AttendanceUserGroup.Commands.Update;

public class UpdateAttendanceUserCommandValidator : AbstractValidator<UpdateAttendanceUserCommand>
{
    public UpdateAttendanceUserCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.VoteSessionId).NotEmpty();
    }
}