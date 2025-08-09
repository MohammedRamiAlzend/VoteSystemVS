using FluentValidation;

namespace Application.Features.VoteGroup.Commands.Create;

public class CreateVoteCommandValidator : AbstractValidator<CreateVoteCommand>
{
    public CreateVoteCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.VoteQuestionOptionId).NotEmpty();
    }
}