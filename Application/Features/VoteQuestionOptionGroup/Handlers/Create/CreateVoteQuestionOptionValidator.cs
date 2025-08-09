using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.VoteQuestionOptionGroup.Handlers.Create
{
    public class CreateVoteQuestionOptionValidator : AbstractValidator<CreateVoteQuestionOptionCommand>
    {
        public CreateVoteQuestionOptionValidator()
        {
            RuleFor(x => x.OptionTitles).NotEmpty().NotNull()
                .WithMessage("At least one option title must be provided.");

            RuleFor(x => x.OptionTitles).NotEmpty().NotNull()
                .WithMessage("Option titles cannot be empty.");
        }
    }
}
