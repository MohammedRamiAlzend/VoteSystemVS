using Application.Features.VoteQuestionOptionGroup.Handlers.Create;
using Application.Features.VoteQuestionOptionGroup.Handlers.Update;
using Application.Features.VoteQuestionOptionGroup.Handlers.Delete;
using Application.Features.VoteQuestionOptionGroup.Queries.GetById;
using Application.Features.VoteQuestionOptionGroup.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace VoteSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VoteQuestionOptionController : ControllerBase
{
    private readonly IMediator _mediator;

    public VoteQuestionOptionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateVoteQuestionOptionCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateVoteQuestionOptionCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetVoteQuestionOptionQuery(id);
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllVoteQuestionOptionQuery();
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteVoteQuestionOptionCommand(id);
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }
}