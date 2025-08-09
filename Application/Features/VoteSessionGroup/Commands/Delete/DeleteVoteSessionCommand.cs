using MediatR;
using Domain.Common.Results;

namespace Application.Features.VoteSessionGroup.Commands.Delete;

public record DeleteVoteSessionCommand(int Id) : IRequest<Result<Deleted>>;