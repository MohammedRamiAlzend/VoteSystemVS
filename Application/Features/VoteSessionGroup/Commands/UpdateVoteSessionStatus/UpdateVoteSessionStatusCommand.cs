using Domain.Common.Results;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.VoteSessionGroup.Commands.UpdateVoteSessionStatus;

public record UpdateVoteSessionStatusCommand(int VoteSessionId, VoteSessionStatus VoteSessionStatus):IRequest<Result<Updated>>;

