using FluentValidation;

namespace Application.Features.VoteGroup.Commands.Update;

public class UpdateVoteCommandValidator : AbstractValidator<UpdateVoteCommand>
{
    public UpdateVoteCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.VoteQuestionOptionId).NotEmpty();
    }
}