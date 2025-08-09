using Application.Features.VoteQuestionGroup.Common;
using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.VoteQuestionOptionGroup.Queries.GetById;

public class GetVoteQuestionOptionQueryHandler
    (
        IUnitOfWork repo,
        ILogger<GetVoteQuestionOptionQueryHandler> logger
    ) : IRequestHandler<GetVoteQuestionOptionQuery, Result<VoteQuestionOptionDto>>
{
    public async Task<Result<VoteQuestionOptionDto>> Handle(GetVoteQuestionOptionQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving vote question option with ID {VoteQuestionOptionId}", request.Id);

        var voteQuestionOptionResult = await repo.VoteQuestionOptionRepository.GetByIdAsync(request.Id);
        if (voteQuestionOptionResult.IsError || voteQuestionOptionResult.Value == null)
        {
            logger.LogWarning("No vote question option found with ID {VoteQuestionOptionId}", request.Id);
            return Error.NotFound(description: $"No vote question option with ID {request.Id} was found");
        }

        var voteQuestionOption = voteQuestionOptionResult.Value;
        var dto = new VoteQuestionOptionDto
        {
            Id = voteQuestionOption.Id,
            Title = voteQuestionOption.Title
        };

        logger.LogInformation("Successfully retrieved vote question option with ID {VoteQuestionOptionId}", request.Id);
        return dto;
    }
}