using MediatR;
using Domain.Common.Results;

namespace Application.Features.VoteQuestionOptionGroup.Handlers.Delete;

public record DeleteVoteQuestionOptionCommand(int Id) : IRequest<Result<Deleted>>;