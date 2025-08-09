using Domain.Common.Results;
using MediatR;

namespace Application.Features.VoteQuestionOptionGroup.Handlers.Update;

public record UpdateVoteQuestionOptionCommand : IRequest<Result<Updated>>
{
    public int VoteQuestionId { get; init; }
    public List<VoteQuestionOptionUpdate> Options { get; init; } = [];

    public record VoteQuestionOptionUpdate
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
    }
}
