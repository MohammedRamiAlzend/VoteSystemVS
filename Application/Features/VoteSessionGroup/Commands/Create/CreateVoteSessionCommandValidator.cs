using Application.Features.VoteSessionGroup.Common;
using FluentValidation;

namespace Application.Features.VoteSessionGroup.Commands.Create;

public class CreateVoteSessionCommandValidator : AbstractValidator<CreateVoteSessionCommand>
{
    public CreateVoteSessionCommandValidator()
    {
        RuleFor(x => x.TopicTitle)
            .NotEmpty().WithMessage("Topic title is required.")
            .MaximumLength(200).WithMessage("Topic title cannot exceed 200 characters.");

        RuleFor(x => x.StartedAt)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(x => x.EndedAt)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThan(x => x.StartedAt).WithMessage("End date must be after start date.");
    }
}
