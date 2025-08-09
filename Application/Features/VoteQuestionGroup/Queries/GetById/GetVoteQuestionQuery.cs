using MediatR;
using Domain.Common.Results;
using Application.Features.VoteQuestionGroup.Common;

namespace Application.Features.VoteQuestionGroup.Queries.GetById;

public record GetVoteQuestionQuery(int Id) : IRequest<Result<VoteQuestionDto>>;