using Application.Features.VoteGroup.Commands.Create;
using Application.Features.VoteGroup.Commands.Update;
using Application.Features.VoteGroup.Commands.Delete;
using Application.Features.VoteGroup.Queries.GetById;
using Application.Features.VoteGroup.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace VoteSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VoteController : ControllerBase
{
    private readonly IMediator _mediator;

    public VoteController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateVoteCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateVoteCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteVoteCommand(id);
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetVoteQuery(id);
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllVoteQuery();
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }
}