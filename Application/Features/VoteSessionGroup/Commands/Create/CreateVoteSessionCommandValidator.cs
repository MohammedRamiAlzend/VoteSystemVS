using Application.Features.VoteSessionGroup.Common;
using FluentValidation;

namespace Application.Features.VoteSessionGroup.Commands.Create;

public class CreateVoteSessionCommandValidator : AbstractValidator<CreateVoteSessionCommand>
{
    public CreateVoteSessionCommandValidator()
    {
        RuleFor(x => x.Dto.TopicTitle)
            .NotEmpty().WithMessage("Topic title is required.")
            .MaximumLength(200).WithMessage("Topic title cannot exceed 200 characters.");

        RuleFor(x => x.Dto.StartedAt)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(x => x.Dto.EndedAt)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThan(x => x.Dto.StartedAt).WithMessage("End date must be after start date.");
    }
}
