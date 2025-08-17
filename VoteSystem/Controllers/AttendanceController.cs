using Application.Features.AttendanceUserGroup.Commands.Create;
using Application.Features.AttendanceUserGroup.Commands.Delete;
using Application.Features.AttendanceUserGroup.Commands.Update;
using Application.Features.AttendanceUserGroup.Queries.GetById;
using Application.Features.AttendanceUserGroup.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace VoteSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AttendanceController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserAttencanceCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateAttendanceUserCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetAttendanceUserQuery(id);
        var result = await mediator.Send(query);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllAttendanceUserQuery();
        var result = await mediator.Send(query);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteAttendanceUserCommand(id);
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result.Errors);
    }
}