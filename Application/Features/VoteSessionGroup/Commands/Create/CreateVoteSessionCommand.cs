using Application.Features.VoteSessionGroup.Common;
using Domain.Entities;
using MediatR;
using Domain.Common.Results;

namespace Application.Features.VoteSessionGroup.Commands.Create;

public record CreateVoteSessionCommand(string? Description, string TopicTitle, DateTime StartedAt, DateTime EndedAt) : IRequest<Result<VoteSessionDto>>;
