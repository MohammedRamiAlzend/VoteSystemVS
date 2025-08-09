using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.VoteSessionGroup.Commands.Create;
using Application.Features.VoteSessionGroup.Common;
using Domain.Common.Results;

namespace VoteSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VoteSessionController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Result<VoteSessionDto>>> Create([FromBody] CreateVoteSessionDto dto)
    {
        return await mediator.Send(new CreateVoteSessionCommand(dto));
    }
}
