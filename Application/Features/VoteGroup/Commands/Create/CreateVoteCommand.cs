using MediatR;
using Domain.Common.Results;
using Application.Features.VoteGroup.Common;

namespace Application.Features.VoteGroup.Commands.Create;

public record CreateVoteCommand(
    int UserId,
    int VoteQuestionOptionId
) : IRequest<Result<Created>>;