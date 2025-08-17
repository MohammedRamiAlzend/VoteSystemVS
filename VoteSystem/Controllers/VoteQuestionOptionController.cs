using Application.Features.VoteQuestionOptionGroup.Handlers.Create;
using Application.Features.VoteQuestionOptionGroup.Handlers.Delete;
using Application.Features.VoteQuestionOptionGroup.Handlers.Update;
using Application.Features.VoteQuestionOptionGroup.Queries.GetAll;
using Application.Features.VoteQuestionOptionGroup.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VoteSystem.Controllers;

[ApiController]
[Route("api/[controller]")]

public class VoteQuestionOptionController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin")]

    public async Task<IActionResult> Create([FromBody] CreateVoteQuestionOptionCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]

    public async Task<IActionResult> Update([FromBody] UpdateVoteQuestionOptionCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetVoteQuestionOptionQuery(id);
        var result = await mediator.Send(query);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllVoteQuestionOptionQuery();
        var result = await mediator.Send(query);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteVoteQuestionOptionCommand(id);
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }
}