using MediatR;
using Domain.Common.Results;

namespace Application.Features.VoteGroup.Commands.Delete;

public record DeleteVoteCommand(int Id) : IRequest<Result<Deleted>>;