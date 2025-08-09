using Application.Features.VoteQuestionGroup.Common;
using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.VoteQuestionOptionGroup.Queries.GetAll;

public class GetAllVoteQuestionOptionQueryHandler
    (
        IUnitOfWork repo,
        ILogger<GetAllVoteQuestionOptionQueryHandler> logger
    ) : IRequestHandler<GetAllVoteQuestionOptionQuery, Result<IEnumerable<VoteQuestionOptionDto>>>
{
    public async Task<Result<IEnumerable<VoteQuestionOptionDto>>> Handle(GetAllVoteQuestionOptionQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving all vote question options");

        var voteQuestionOptionsResult = await repo.VoteQuestionOptionRepository.GetAllAsync();
        if (voteQuestionOptionsResult.IsError)
        {
            logger.LogError("Failed to retrieve vote question options: {Errors}", voteQuestionOptionsResult.Errors);
            return voteQuestionOptionsResult.Errors;
        }

        var voteQuestionOptions = voteQuestionOptionsResult.Value;
        var dtos = voteQuestionOptions.Select(option => new VoteQuestionOptionDto
        {
            Id = option.Id,
            Title = option.Title
        }).ToList();

        logger.LogInformation("Successfully retrieved {Count} vote question options", dtos.Count);
        return dtos;
    }
}