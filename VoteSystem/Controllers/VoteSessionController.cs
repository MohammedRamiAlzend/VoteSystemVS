using Application.Features.VoteSessionGroup.Commands.Create;
using Application.Features.VoteSessionGroup.Commands.UpdateVoteSessionStatus;
using Application.Features.VoteSessionGroup.Commands.Delete;
using Application.Features.VoteSessionGroup.Queries.GetById;
using Application.Features.VoteSessionGroup.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace VoteSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VoteSessionController : ControllerBase
{
    private readonly IMediator _mediator;

    public VoteSessionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateVoteSessionCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result ) : BadRequest(result.Errors);
    }

    [HttpPut("status")]
    public async Task<IActionResult> UpdateStatus(UpdateVoteSessionStatusCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetVoteSessionQuery(id);
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllVoteSessionQuery();
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteVoteSessionCommand(id);
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }
}
