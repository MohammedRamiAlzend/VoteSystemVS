using Application.Features.VoteSessionGroup.Common;
using Domain.Entities;
using Infrastructure.Repositories.Abstractions;
using MediatR;
using Domain.Common.Results;
using Microsoft.Extensions.Logging;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Features.VoteSessionGroup.Commands.Create;

public class CreateVoteSessionCommandHandler
    (
        IUnitOfWork repo,
        IValidator<CreateVoteSessionCommand> validator,
        ILogger<CreateVoteSessionCommandHandler> logger,
        IHttpContextAccessor context
    )
    : IRequestHandler<CreateVoteSessionCommand, Result<VoteSessionDto>>
{
    public async Task<Result<VoteSessionDto>> Handle(CreateVoteSessionCommand request, CancellationToken cancellationToken)
    {
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);
        if (validatorResult.IsValid)
        {
            validatorResult.Errors.MapFromFluentValidationErrors();
        }

        var voteSession = new VoteSession
        {
            Description = request.Description,
            StartedAt = request.StartedAt,
            TopicTitle = request.TopicTitle,
            EndedAt = request.EndedAt
        };
        var addingResult = await repo.VoteSessionRepository.AddAsync(voteSession);
        var commitChanges = await repo.SaveChangesAsync(cancellationToken);
        if (addingResult.IsError)
        {
            return addingResult.TopError;
        }
        if (commitChanges.IsError)
        {
            return commitChanges.TopError;
        }

        // Get admin from context to log action
        var getAdminFromContext = context.HttpContext?.User;
        string performedBy = "Unknown Admin";
        if (getAdminFromContext != null && (getAdminFromContext.IsInRole("Admin") || getAdminFromContext.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin")))
        {
            performedBy = getAdminFromContext.Identity.Name ?? "Unknown Admin";
        }

        // Log the vote session creation
        var systemLog = new SystemLog
        {
            Action = $"Vote session '{request.TopicTitle}' created",
            PerformedBy = performedBy,
            TimeStamp = DateTime.UtcNow
            

        };

        await repo.SystemLogRepository.AddAsync(systemLog);
        await repo.SaveChangesAsync(cancellationToken);

        return new VoteSessionDto
        {
            Id = voteSession.Id,
            Description = voteSession.Description,
            VoteSessionStatus = voteSession.VoteSessionStatus,
            StartedAt = voteSession.StartedAt,
            TopicTitle = voteSession.TopicTitle,
            EndedAt = voteSession.EndedAt
        };
    }




}
