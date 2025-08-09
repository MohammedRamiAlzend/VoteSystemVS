using Application.Features.VoteSessionGroup.Common;
using Domain.Entities;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Domain.Common.Results;
using Microsoft.Extensions.Logging;

namespace Application.Features.VoteSessionGroup.Commands.Create;
public record CreateVoteSessionCommand(CreateVoteSessionDto Dto) : IRequest<Result<VoteSessionDto>>;

public class CreateVoteSessionCommandHandler
    (
        IUnitOfWork repo,
        ILogger<CreateVoteSessionCommandHandler> logger
    )
    : IRequestHandler<CreateVoteSessionCommand, Result<VoteSessionDto>>
{
    public async Task<Result<VoteSessionDto>> Handle(CreateVoteSessionCommand request, CancellationToken cancellationToken)
    {
        var voteSession = new VoteSession
        {
            Description = request.Dto.Description,
            IsActive = request.Dto.IsActive,
            StartedAt = request.Dto.StartedAt,
            TopicTitle = request.Dto.TopicTitle,
            EndedAt = request.Dto.EndedAt
        };
        var addingResult = await repo.VoteSessionRepository.AddAsync(voteSession);
        var commitChanges = await repo.SaveChangesAsync();
        if (addingResult.IsError)
        {
            return addingResult.TopError;
        }
        if (commitChanges.IsError)
        {
            return commitChanges.TopError;
        }
        return new VoteSessionDto
        {
            Id = voteSession.Id,
            Description = voteSession.Description,
            IsActive = voteSession.IsActive,
            StartedAt = voteSession.StartedAt,
            TopicTitle = voteSession.TopicTitle,
            EndedAt = voteSession.EndedAt
        };
    }
}
