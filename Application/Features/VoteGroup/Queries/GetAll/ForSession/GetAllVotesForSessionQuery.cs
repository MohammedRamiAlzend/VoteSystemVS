using Application.Features.VoteGroup.Common;
using Domain.Common.Results;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Features.VoteGroup.Queries.GetAll.ForSession;

public record GetAllVotesForSessionQuery(int SessionId) : IRequest<Result<VotesForSessionDTO>>;