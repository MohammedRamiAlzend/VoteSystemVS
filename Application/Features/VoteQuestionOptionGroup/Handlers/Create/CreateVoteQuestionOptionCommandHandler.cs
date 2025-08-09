using Application.Features.VoteQuestionOptionGroup.Handlers.Create;
using Domain.Common.Results;
using Domain.Entities;
using FluentValidation;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.VoteQuestionGroup.Handlers.Create;



public class CreateVoteQuestionOptionCommandHandler
    (
        IUnitOfWork repo,
        ILogger<CreateVoteQuestionOptionCommandHandler> logger,
        IValidator<CreateVoteQuestionOptionCommand> validator
    ) : IRequestHandler<CreateVoteQuestionOptionCommand, Result<Created>>
{
    public async Task<Result<Created>> Handle(CreateVoteQuestionOptionCommand request, CancellationToken cancellationToken)
    {
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        if (validatorResult.IsValid)
        {
            validatorResult.Errors.MapFromFluentValidationErrors();
        }
        logger.LogInformation("Adding vote question options for VoteQuestionId {VoteQuestionId}", request.VoteQuestionId);

        var voteQuestionResult = await repo.VoteQuestionRepository.GetByIdAsync(request.VoteQuestionId);
        if (voteQuestionResult.IsError || voteQuestionResult.Value == null)
        {
            logger.LogWarning("No vote question found with ID {VoteQuestionId}", request.VoteQuestionId);
            return Error.NotFound(description: $"No vote question with ID {request.VoteQuestionId} was found");
        }

        List<VoteQuestionOption> voteQuestionOptions = [.. request.OptionTitles.Select(title => new VoteQuestionOption
        {
            VoteQuestionId = request.VoteQuestionId,
            Title = title,
        })];

        logger.LogInformation("Adding {OptionCount} vote question options for VoteQuestionId {VoteQuestionId}", voteQuestionOptions.Count, request.VoteQuestionId);
        var addOptionsResult = await repo.VoteQuestionOptionRepository.AddRangeAsync(voteQuestionOptions);
        if (addOptionsResult.IsError)
        {
            logger.LogError("Failed to add vote question options: {Errors}", addOptionsResult.Errors);
            return addOptionsResult.Errors;
        }

        // Save changes
        var saveResult = await repo.SaveChangesAsync(cancellationToken);
        if (saveResult.IsError)
        {
            logger.LogError("Failed to save vote question options: {Errors}", saveResult.Errors);
            return saveResult.Errors;
        }

        logger.LogInformation("Successfully added {OptionCount} vote question options for VoteQuestionId {VoteQuestionId}", voteQuestionOptions.Count, request.VoteQuestionId);
        return Result.Created;
    }
}