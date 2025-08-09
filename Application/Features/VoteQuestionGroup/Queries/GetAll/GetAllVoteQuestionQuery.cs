using MediatR;
using Domain.Common.Results;
using Application.Features.VoteQuestionGroup.Common;

namespace Application.Features.VoteQuestionGroup.Queries.GetAll;

public record GetAllVoteQuestionQuery() : IRequest<Result<IEnumerable<VoteQuestionDto>>>;