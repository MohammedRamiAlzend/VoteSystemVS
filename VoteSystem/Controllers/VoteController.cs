using Application.Features.VoteGroup.Commands.Create;
using Application.Features.VoteGroup.Commands.Delete;
using Application.Features.VoteGroup.Commands.Update;
using Application.Features.VoteGroup.Queries.GetAll;
using Application.Features.VoteGroup.Queries.GetAll.ForSession;
using Application.Features.VoteGroup.Queries.GetAll.ForUser;
using Application.Features.VoteGroup.Queries.GetById;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace VoteSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VoteController : ControllerBase
{
    private readonly IMediator _mediator;

    public VoteController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVoteCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateVoteCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteVoteCommand(id);
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetVoteQuery(id);
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpGet("get-all-vote")]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllVotesQuery();
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }


    [HttpGet("get-all-votes-for-session")]
    public async Task<IActionResult> GetAllVotesForSession(int sessionId)
    {
        var query = new GetAllVotesForSessionQuery(sessionId);
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpGet("get-all-votes-for-user")]
    public async Task<IActionResult> GetAllVotesForUser(int userID)
    {
        var query = new GetAllVotesForUserQuery(userID);
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }
}