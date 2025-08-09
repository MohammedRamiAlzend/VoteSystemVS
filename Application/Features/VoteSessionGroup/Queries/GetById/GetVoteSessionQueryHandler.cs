using Application.Features.VoteSessionGroup.Common;
using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.VoteSessionGroup.Queries.GetById;

public class GetVoteSessionQueryHandler
    (
        IUnitOfWork repo,
        ILogger<GetVoteSessionQueryHandler> logger
    ) : IRequestHandler<GetVoteSessionQuery, Result<VoteSessionDto>>
{
    public async Task<Result<VoteSessionDto>> Handle(GetVoteSessionQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving vote session with ID {VoteSessionId}", request.Id);

        var voteSessionResult = await repo.VoteSessionRepository.GetByIdAsync(request.Id);
        if (voteSessionResult.IsError || voteSessionResult.Value == null)
        {
            logger.LogWarning("No vote session found with ID {VoteSessionId}", request.Id);
            return Error.NotFound(description: $"No vote session with ID {request.Id} was found");
        }

        var voteSession = voteSessionResult.Value;
        var dto = new VoteSessionDto
        {
            Id = voteSession.Id,
            Description = voteSession.Description,
            StartedAt = voteSession.StartedAt,
            TopicTitle = voteSession.TopicTitle,
            EndedAt = voteSession.EndedAt,
            VoteSessionStatus = voteSession.VoteSessionStatus
        };

        logger.LogInformation("Successfully retrieved vote session with ID {VoteSessionId}", request.Id);
        return dto;
    }
}