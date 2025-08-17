using MediatR;
using Domain.Common.Results;
using Application.Features.VoteGroup.Common;

namespace Application.Features.VoteGroup.Queries.GetById;

public record GetVoteQuery(int Id) : IRequest<Result<VoteDTO>>;