using MediatR;
using Domain.Common.Results;

namespace Application.Features.VoteQuestionGroup.Handlers.Delete;

public record DeleteVoteQuestionCommand(int Id) : IRequest<Result<Deleted>>;