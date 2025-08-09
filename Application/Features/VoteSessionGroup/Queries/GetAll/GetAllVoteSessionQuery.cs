using MediatR;
using Domain.Common.Results;
using Application.Features.VoteSessionGroup.Common;

namespace Application.Features.VoteSessionGroup.Queries.GetAll;

public record GetAllVoteSessionQuery() : IRequest<Result<IEnumerable<VoteSessionDto>>>;