using Application.Features.VoteQuestionGroup.Common;
using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.VoteQuestionGroup.Queries.GetAll;

public class GetAllVoteQuestionQueryHandler
    (
        IUnitOfWork repo,
        ILogger<GetAllVoteQuestionQueryHandler> logger
    ) : IRequestHandler<GetAllVoteQuestionQuery, Result<IEnumerable<VoteQuestionDto>>>
{
    public async Task<Result<IEnumerable<VoteQuestionDto>>> Handle(GetAllVoteQuestionQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving all vote questions");

        var voteQuestionsResult = await repo.VoteQuestionRepository.GetAllAsync(include: query => query.Include(vq => vq.Options));
        if (voteQuestionsResult.IsError)
        {
            logger.LogError("Failed to retrieve vote questions: {Errors}", voteQuestionsResult.Errors);
            return voteQuestionsResult.Errors;
        }

        var voteQuestions = voteQuestionsResult.Value;
        var dtos = voteQuestions.Select(voteQuestion => new VoteQuestionDto
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
        }).ToList();

        logger.LogInformation("Successfully retrieved {Count} vote questions", dtos.Count);
        return dtos;
    }
}