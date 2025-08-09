using FluentValidation;

namespace Application.Features.VoteQuestionGroup.Handlers.Update;

public class UpdateVoteQuestionCommandValidator : AbstractValidator<UpdateVoteQuestionCommand>
{
    public UpdateVoteQuestionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(250);
        RuleFor(x => x.StartedAt).NotEmpty();
        RuleFor(x => x.EndedAt).NotEmpty().GreaterThanOrEqualTo(x => x.StartedAt);
        RuleFor(x => x.VoteSessionId).NotEmpty();
        RuleForEach(x => x.Options).ChildRules(option =>
        {
            option.RuleFor(x => x.Id).NotEmpty();
            option.RuleFor(x => x.Title).NotEmpty().MaximumLength(250);
        });
    }
}