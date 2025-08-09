using MediatR;
using Domain.Common.Results;

namespace Application.Features.VoteQuestionGroup.Handlers.Update;

public record UpdateVoteQuestionCommand(
    int Id,
    string Title,
    DateTime StartedAt,
    DateTime EndedAt,
    int VoteSessionId,
    List<UpdateVoteQuestionOptionCommand> Options
) : IRequest<Result<Updated>>;

public record UpdateVoteQuestionOptionCommand(
    int Id,
    string Title
);