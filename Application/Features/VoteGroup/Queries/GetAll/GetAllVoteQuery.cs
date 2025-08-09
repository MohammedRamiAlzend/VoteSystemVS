using MediatR;
using Domain.Common.Results;
using Application.Features.VoteGroup.Common;

namespace Application.Features.VoteGroup.Queries.GetAll;

public record GetAllVoteQuery() : IRequest<Result<IEnumerable<VoteDto>>>;