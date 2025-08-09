using Application.Features.VoteQuestionGroup.Common;
using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.VoteQuestionGroup.Queries.GetById;

public class GetVoteQuestionQueryHandler
    (
        IUnitOfWork repo,
        ILogger<GetVoteQuestionQueryHandler> logger
    ) : IRequestHandler<GetVoteQuestionQuery, Result<VoteQuestionDto>>
{
    public async Task<Result<VoteQuestionDto>> Handle(GetVoteQuestionQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving vote question with ID {VoteQuestionId}", request.Id);

        var voteQuestionResult = await repo.VoteQuestionRepository.GetByIdAsync(request.Id, query => query.Include(vq => vq.Options));
        if (voteQuestionResult.IsError || voteQuestionResult.Value == null)
        {
            logger.LogWarning("No vote question found with ID {VoteQuestionId}", request.Id);
            return Error.NotFound(description: $"No vote question with ID {request.Id} was found");
        }

        var voteQuestion = voteQuestionResult.Value;
        var dto = new VoteQuestionDto
        {
            Id = voteQuestion.Id,
            Title = voteQuestion.Title,
            StartedAt = voteQuestion.StartedAt,
            EndedAt = voteQuestion.EndedAt,
            VoteSessionId = voteQuestion.VoteSessionId,
            Options = voteQuestion.Options.Select(o => new VoteQuestionOptionDto
            {
                Id = o.Id,
                Title = o.Title
            }).ToList()
        };

        logger.LogInformation("Successfully retrieved vote question with ID {VoteQuestionId}", request.Id);
        return dto;
    }
}