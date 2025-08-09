using Application.Features.VoteGroup.Common;
using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.VoteGroup.Queries.GetById;

public class GetVoteQueryHandler
    (
        IUnitOfWork repo,
        ILogger<GetVoteQueryHandler> logger
    ) : IRequestHandler<GetVoteQuery, Result<VoteDto>>
{
    public async Task<Result<VoteDto>> Handle(GetVoteQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving vote with ID {VoteId}", request.Id);

        var voteResult = await repo.VoteRepository.GetByIdAsync(request.Id, query => query.Include(v => v.User).Include(v => v.VoteQuestionOption));
        if (voteResult.IsError || voteResult.Value == null)
        {
            logger.LogWarning("No vote found with ID {VoteId}", request.Id);
            return Error.NotFound(description: $"No vote with ID {request.Id} was found");
        }

        var vote = voteResult.Value;
        var dto = new VoteDto
        {
            Id = vote.Id,
            VotedAt = vote.VotedAt,
            UserId = vote.UserId,
            VoteQuestionOptionId = vote.VoteQuestionOptionId
        };

        logger.LogInformation("Successfully retrieved vote with ID {VoteId}", request.Id);
        return dto;
    }
}