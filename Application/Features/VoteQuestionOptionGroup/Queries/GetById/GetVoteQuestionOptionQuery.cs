using MediatR;
using Domain.Common.Results;
using Application.Features.VoteQuestionGroup.Common;

namespace Application.Features.VoteQuestionOptionGroup.Queries.GetById;

public record GetVoteQuestionOptionQuery(int Id) : IRequest<Result<VoteQuestionOptionDto>>;