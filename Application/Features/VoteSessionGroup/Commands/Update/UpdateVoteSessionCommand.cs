using MediatR;
using Domain.Common.Results;
using Application.Features.VoteSessionGroup.Common;

namespace Application.Features.VoteSessionGroup.Commands.Update;

public record UpdateVoteSessionCommand(
    int Id,
    string Description,
    DateTime StartedAt,
    string TopicTitle,
    DateTime EndedAt
) : IRequest<Result<Updated>>;