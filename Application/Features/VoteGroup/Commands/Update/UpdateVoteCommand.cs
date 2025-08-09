using MediatR;
using Domain.Common.Results;

namespace Application.Features.VoteGroup.Commands.Update;

public record UpdateVoteCommand(
    int Id,
    int UserId,
    int VoteQuestionOptionId
) : IRequest<Result<Updated>>;