using Application.Features.VoteGroup.Common;
using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.VoteGroup.Queries.GetAll;

public class GetAllVoteQueryHandler
    (
        IUnitOfWork repo,
        ILogger<GetAllVoteQueryHandler> logger
    ) : IRequestHandler<GetAllVoteQuery, Result<IEnumerable<VoteDto>>>
{
    public async Task<Result<IEnumerable<VoteDto>>> Handle(GetAllVoteQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving all votes");

        var votesResult = await repo.VoteRepository.GetAllAsync(include: query => query.Include(v => v.User).Include(v => v.VoteQuestionOption));
        if (votesResult.IsError)
        {
            logger.LogError("Failed to retrieve votes: {Errors}", votesResult.Errors);
            return votesResult.Errors;
        }

        var votes = votesResult.Value;
        var dtos = votes.Select(vote => new VoteDto
        {
            Id = vote.Id,
            VotedAt = vote.VotedAt,
            UserId = vote.UserId,
            VoteQuestionOptionId = vote.VoteQuestionOptionId
        }).ToList();

        logger.LogInformation("Successfully retrieved {Count} votes", dtos.Count);
        return dtos;
    }
}