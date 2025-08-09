using Domain.Common.Results;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.VoteSessionGroup.Commands.UpdateVoteSessionStatus;

public class UpdateVoteSessionStatusCommandHandler(
    IUnitOfWork repo,
    ILogger<UpdateVoteSessionStatusCommandHandler> logger)
        : IRequestHandler<UpdateVoteSessionStatusCommand, Result<Updated>>
{

    /// <summary>
    /// Handles the command to update the status of a vote session.
    /// </summary>
    /// <param name="request">The command containing the vote session ID and new status.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>A Result indicating success or failure.</returns>
    public async Task<Result<Updated>> Handle(UpdateVoteSessionStatusCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing UpdateVoteSessionStatus for VoteSessionId {VoteSessionId}", request.VoteSessionId);

        var getVoteSession = await repo.VoteSessionRepository.GetByIdAsync(request.VoteSessionId);
        if (getVoteSession.IsError)
        {
            logger.LogWarning("Failed to retrieve VoteSession with ID {VoteSessionId}: {Errors}", request.VoteSessionId, getVoteSession.Errors);
            return getVoteSession.Errors;
        }
        if (getVoteSession.Value is null)
        {
            logger.LogWarning("VoteSession with ID {VoteSessionId} not found", request.VoteSessionId);
            return Error.NotFound(description: "Vote session does not exist");
        }
        getVoteSession.Value.VoteSessionStatus = request.VoteSessionStatus;
        var updateResult = await repo.VoteSessionRepository.UpdateAsync(getVoteSession.Value);
        if (updateResult.IsError)
        {
            logger.LogError("Failed to update VoteSession with ID {VoteSessionId}: {Errors}", request.VoteSessionId, updateResult.Errors);
            return updateResult.Errors;
        }
        var saveResult = await repo.SaveChangesAsync(cancellationToken);
        if (saveResult.IsError)
        {
            logger.LogError("Failed to save changes for VoteSessionId {VoteSessionId}: {Errors}", request.VoteSessionId, saveResult.Errors);
            return saveResult.Errors;
        }
        logger.LogInformation("Successfully updated VoteSessionStatus for VoteSessionId {VoteSessionId} to {Status}", request.VoteSessionId, request.VoteSessionStatus);
        
        return Result.Updated;
    }
}