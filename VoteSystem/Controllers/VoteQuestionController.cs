using Application.Features.VoteQuestionGroup.Handlers.Create;
using Application.Features.VoteQuestionGroup.Handlers.Update;
using Application.Features.VoteQuestionGroup.Handlers.Delete;
using Application.Features.VoteQuestionGroup.Queries.GetById;
using Application.Features.VoteQuestionGroup.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace VoteSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VoteQuestionController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateVoteQuestionCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update([FromBody] UpdateVoteQuestionCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetVoteQuestionQuery(id);
        var result = await mediator.Send(query);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllVoteQuestionQuery();
        var result = await mediator.Send(query);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteVoteQuestionCommand(id);
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }
}