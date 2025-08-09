using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.VoteQuestionOptionGroup.Handlers.Update
{
    public class UpdateVoteQuestionOptionValidator : AbstractValidator<UpdateVoteQuestionOptionCommand>
    {
        public UpdateVoteQuestionOptionValidator()
        {
            RuleFor(x=>x.Options).NotEmpty().NotNull()
                .WithMessage("At least one option must be provided for update.");

            RuleFor(x => x.Options).Must(
                x =>
                {
                    if(x.Any(o=>o.Id<= 0))
                    {
                        return false;
                    }
                    else
                        return true;
                }).WithMessage("All option IDs must be positive integers.");
            RuleFor(x => x.Options).Must(
               x =>
               {
                   if (x.Any(o => string.IsNullOrWhiteSpace(o.Title)))
                   {
                       return false;
                   }
                   else
                       return true;
               }).WithMessage("Option titles cannot be empty.");
        }
    }
}
