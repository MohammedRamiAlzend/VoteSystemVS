using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.VoteSessionGroup.Commands.UpdateVoteSessionStatus
{
    public class UpdateVoteSessionStatusValidator : AbstractValidator<UpdateVoteSessionStatusCommand>
    {
        public UpdateVoteSessionStatusValidator()
        {
            RuleFor(x => x.VoteSessionStatus).IsInEnum().WithMessage("Invalid vote session status");
        }
    }
}
