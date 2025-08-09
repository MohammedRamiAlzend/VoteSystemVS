using Application.Features.VoteSessionGroup.Commands.Create;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.VoteQuestionGroup.Handlers.Create
{
    public class CreateVoteQuestionValidator : AbstractValidator<CreateVoteQuestionCommand>
    {
        public CreateVoteQuestionValidator()
        {
            RuleFor(x => x.Title).NotEmpty().NotNull().WithMessage("title should not be empty");
            RuleFor(x => x.EndedAt).GreaterThan(x => x.StartedAt).WithMessage("start date should be less than end date");
        }
    }
}
