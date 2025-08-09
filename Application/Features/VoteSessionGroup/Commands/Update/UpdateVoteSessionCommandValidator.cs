using FluentValidation;

namespace Application.Features.VoteSessionGroup.Commands.Update;

public class UpdateVoteSessionCommandValidator : AbstractValidator<UpdateVoteSessionCommand>
{
    public UpdateVoteSessionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.StartedAt).NotEmpty();
        RuleFor(x => x.TopicTitle).NotEmpty().MaximumLength(250);
        RuleFor(x => x.EndedAt).NotEmpty().GreaterThanOrEqualTo(x => x.StartedAt);
    }
}