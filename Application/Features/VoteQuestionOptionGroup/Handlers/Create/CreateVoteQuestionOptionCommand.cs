using Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.VoteQuestionOptionGroup.Handlers.Create;
public record CreateVoteQuestionOptionCommand : IRequest<Result<Created>>
{
    public int VoteQuestionId { get; init; }
    public List<string> OptionTitles { get; init; } = [];
}