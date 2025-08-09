using MediatR;
using Domain.Common.Results;
using Application.Features.VoteSessionGroup.Common;

namespace Application.Features.VoteSessionGroup.Queries.GetById;

public record GetVoteSessionQuery(int Id) : IRequest<Result<VoteSessionDto>>;