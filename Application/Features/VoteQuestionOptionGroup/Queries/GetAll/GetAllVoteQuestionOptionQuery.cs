using MediatR;
using Domain.Common.Results;
using Application.Features.VoteQuestionGroup.Common;

namespace Application.Features.VoteQuestionOptionGroup.Queries.GetAll;

public record GetAllVoteQuestionOptionQuery() : IRequest<Result<IEnumerable<VoteQuestionOptionDto>>>;