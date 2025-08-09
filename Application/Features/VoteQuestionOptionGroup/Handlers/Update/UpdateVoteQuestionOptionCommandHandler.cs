using Domain.Common.Results;
using Domain.Entities;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.VoteQuestionOptionGroup.Handlers.Update;

public class UpdateVoteQuestionOptionCommandHandler
    (
        IUnitOfWork repo,
        ILogger<UpdateVoteQuestionOptionCommandHandler> logger
    ) : IRequestHandler<UpdateVoteQuestionOptionCommand, Result<Updated>>
{
    public async Task<Result<Updated>> Handle(UpdateVoteQuestionOptionCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating vote question options for VoteQuestionId {VoteQuestionId}", request.VoteQuestionId);


        var voteQuestionResult = await repo.VoteQuestionRepository.GetByIdAsync(request.VoteQuestionId);
        if (voteQuestionResult.IsError || voteQuestionResult.Value == null)
        {
            logger.LogWarning("No vote question found with ID {VoteQuestionId}", request.VoteQuestionId);
            return Error.NotFound(description: $"No vote question with ID {request.VoteQuestionId} was found");
        }

        var optionIds = request.Options.Select(o => o.Id).ToList();
        List<VoteQuestionOption> voteQuestionOptions = [];
        foreach (var item in request.Options)
        {
            var existingOptionsResult = await repo.VoteQuestionOptionRepository.GetByIdAsync(item.Id);
            if (existingOptionsResult.IsError)
            {
                logger.LogError("Failed to retrieve vote question options: {Errors}", existingOptionsResult.Errors);
                return existingOptionsResult.Errors;
            }
            voteQuestionOptions.Add(existingOptionsResult.Value!);
        }

        var existingOptions = voteQuestionOptions;
        if (existingOptions.Count != request.Options.Count)
        {
            var missingIds = optionIds.Except(existingOptions.Select(o => o.Id)).ToList();
            logger.LogWarning("Some option IDs not found for VoteQuestionId {VoteQuestionId}: {MissingIds}", request.VoteQuestionId, string.Join(", ", missingIds));
            return Error.NotFound(description: $"One or more options not found: {string.Join(", ", missingIds)}");
        }

        if (existingOptions.Any(o => o.VoteQuestionId != request.VoteQuestionId))
        {
            logger.LogWarning("Some options do not belong to VoteQuestionId {VoteQuestionId}", request.VoteQuestionId);
            return Error.Conflict(description: "All options must belong to the specified vote question.");
        }

        foreach (var optionUpdate in request.Options)
        {
            var option = existingOptions.First(o => o.Id == optionUpdate.Id);
            option.Title = optionUpdate.Title;
        }

        var updateResult = await repo.VoteQuestionOptionRepository.UpdateRangeAsync(existingOptions);
        if (updateResult.IsError)
        {
            logger.LogError("Failed to update vote question options: {Errors}", updateResult.Errors);
            return updateResult.Errors;
        }

        var saveResult = await repo.SaveChangesAsync(cancellationToken);
        if (saveResult.IsError)
        {
            logger.LogError("Failed to save updated vote question options: {Errors}", saveResult.Errors);
            return saveResult.Errors;
        }

        logger.LogInformation("Successfully updated {OptionCount} vote question options for VoteQuestionId {VoteQuestionId}", request.Options.Count, request.VoteQuestionId);
        return Result.Updated;
    }
}