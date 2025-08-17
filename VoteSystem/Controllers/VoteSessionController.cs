using Application.Features.VoteSessionGroup.Commands;
using Application.Features.VoteSessionGroup.Commands.Create;
using Application.Features.VoteSessionGroup.Commands.Delete;
using Application.Features.VoteSessionGroup.Commands.Update;
using Application.Features.VoteSessionGroup.Commands.UpdateVoteSessionStatus;
using Application.Features.VoteSessionGroup.Queries.GetAll;
using Application.Features.VoteSessionGroup.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VoteSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VoteSessionController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize("Admin")]
    public async Task<IActionResult> Create([FromBody]CreateVoteSessionCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpPut]
    [Authorize("Admin")]
    public async Task<IActionResult> Update([FromBody] UpdateVoteSessionCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpPut("status")]
    [Authorize("Admin")]
    public async Task<IActionResult> UpdateStatus(UpdateVoteSessionStatusCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetVoteSessionQuery(id);
        var result = await mediator.Send(query);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllVoteSessionQuery();
        var result = await mediator.Send(query);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteVoteSessionCommand(id);
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpPost("{id}/invite-all-via-email")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> InviteAll(int voteSessionId)
    {
        var command = new SendVoteSessionInviteLinksCommand(voteSessionId);
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpPost("{id}/invite-user-via-email")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> InviteUser(int voteSessionId, [FromQuery] string Useremail)
    {
        var command = new SendVoteSessionInviteLinkCommand(voteSessionId,Useremail);
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }
}
