using Application.Features.VoteGroup.Common;
using Application.Features.VoteGroup.Queries.GetAll.ForSession;
using AutoMapper;
using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.VoteGroup.Queries.GetAll;

public class GetAllVoteForSessionQueryHandler
    (
        IUnitOfWork repo,
        ILogger<GetAllVoteQueryHandler> logger,
        IMapper mapper
    ) : IRequestHandler<GetAllVotesForSessionQuery, Result<VotesForSessionDTO>>
{
    public async Task<Result<VotesForSessionDTO>> Handle(GetAllVotesForSessionQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving all votes");

        var voteSessionResult = await repo.VoteSessionRepository.GetByIdAsync(request.SessionId,
            include: i => i.Include(x => x.Questions).ThenInclude(x => x.Options).ThenInclude(x => x.Votes));
        if (voteSessionResult.IsError)
        {
            logger.LogError("Failed to retrieve votes: {Errors}", voteSessionResult.Errors);
            return voteSessionResult.Errors;
        }
        if (voteSessionResult.Value is null)
        {
            return Error.NotFound();
        }

        var votes = mapper.Map<VotesForSessionDTO>(voteSessionResult.Value);
 

        return votes;
    }
}


public class VotesForSessionDTO
{
    public required int VoteSessionId { get; set; }
    public required List<QuestionDTO> Questions{ get; set;}
}
public class QuestionDTO
{
    public required List<QuestionOptionDTO> Options { get; set; }
}
public class QuestionOptionDTO
{
    public required List<VoteDTO> Votes { get; set; }
}
