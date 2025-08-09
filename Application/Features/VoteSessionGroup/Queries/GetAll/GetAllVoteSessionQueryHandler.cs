using Application.Features.VoteSessionGroup.Common;
using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.VoteSessionGroup.Queries.GetAll;

public class GetAllVoteSessionQueryHandler
    (
        IUnitOfWork repo,
        ILogger<GetAllVoteSessionQueryHandler> logger
    ) : IRequestHandler<GetAllVoteSessionQuery, Result<IEnumerable<VoteSessionDto>>>
{
    public async Task<Result<IEnumerable<VoteSessionDto>>> Handle(GetAllVoteSessionQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving all vote sessions");

        var voteSessionsResult = await repo.VoteSessionRepository.GetAllAsync();
        if (voteSessionsResult.IsError)
        {
            logger.LogError("Failed to retrieve vote sessions: {Errors}", voteSessionsResult.Errors);
            return voteSessionsResult.Errors;
        }

        var voteSessions = voteSessionsResult.Value;
        var dtos = voteSessions.Select(voteSession => new VoteSessionDto
        {
            Id = voteSession.Id,
            Description = voteSession.Description,
            StartedAt = voteSession.StartedAt,
            TopicTitle = voteSession.TopicTitle,
            EndedAt = voteSession.EndedAt,
            VoteSessionStatus = voteSession.VoteSessionStatus
        }).ToList();

        logger.LogInformation("Successfully retrieved {Count} vote sessions", dtos.Count);
        return dtos;
    }
}