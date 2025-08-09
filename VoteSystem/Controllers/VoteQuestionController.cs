using Application.Features.VoteQuestionGroup.Handlers.Create;
using Application.Features.VoteQuestionGroup.Handlers.Update;
using Application.Features.VoteQuestionGroup.Handlers.Delete;
using Application.Features.VoteQuestionGroup.Queries.GetById;
using Application.Features.VoteQuestionGroup.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace VoteSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VoteQuestionController : ControllerBase
{
    private readonly IMediator _mediator;

    public VoteQuestionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateVoteQuestionCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromForm] UpdateVoteQuestionCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetVoteQuestionQuery(id);
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllVoteQuestionQuery();
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteVoteQuestionCommand(id);
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }
}