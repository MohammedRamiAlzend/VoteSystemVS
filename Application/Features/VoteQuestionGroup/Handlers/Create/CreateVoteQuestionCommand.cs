using Domain.Common.Results;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.VoteQuestionGroup.Handlers.Create
{
    public record CreateVoteQuestionCommand(string Title, string? Description, DateTime StartedAt, DateTime EndedAt, int VoteSessionId) : IRequest<Result<Created>>
    {
        public ICollection<CreateVoteQuestionOptionDto> Options { get; set; } = [];
    }

    public class CreateVoteQuestionOptionDto
    {
        public string Title { get; set; }
    }
}
