using Application.Features.VoteGroup.Common;
using Domain.Common.Results;
using Domain.Entities;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Features.VoteGroup.Queries.GetAll.ForUser;


public record GetAllVotesForUserQuery(int UserId) : IRequest<Result<GetAllVotesForUserDTO>>;



public class GetAllVotesForUserQueryHandler
    (
        IUnitOfWork repos,
        ILogger<GetAllVotesForUserQueryHandler> logger
    ) : IRequestHandler<GetAllVotesForUserQuery, Result<GetAllVotesForUserDTO>>
{
    public async Task<Result<GetAllVotesForUserDTO>> Handle(GetAllVotesForUserQuery request, CancellationToken cancellationToken)
    {
        var getUserVote = await repos.UserRepository.GetByIdAsync(request.UserId, i => i.Include(x => x.Votes));
        
        if (getUserVote.IsError)
            return getUserVote.Errors;
        
        if (getUserVote.Value is null)
            return Error.NotFound();
        
        if (getUserVote.Value.Votes.Count == 0)
            return Error.NotFound();
        
        List<VoteDTO> dtoVotes = [.. getUserVote.Value.Votes.Select(x => new VoteDTO {
            Id = x.Id,
            UserId = x.UserId,
            VotedAt = DateTime.UtcNow,
            VoteQuestionOptionId= x.VoteQuestionOptionId
        })];
        return new GetAllVotesForUserDTO()
        {
            UserId = request.UserId,
            Votes = dtoVotes
        };

    }
}
public class GetAllVotesForUserDTO
{
    public required int UserId { get; set; }
    public required List<VoteDTO> Votes { get; set; }
}