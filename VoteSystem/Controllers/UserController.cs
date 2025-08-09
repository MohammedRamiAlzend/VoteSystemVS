using Application.Features.UserGroup.Commands.Create;
using Application.Features.UserGroup.Commands.Update;
using Application.Features.UserGroup.Commands.Delete;
using Application.Features.UserGroup.Queries.GetById;
using Application.Features.UserGroup.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace VoteSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateUserCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteUserCommand(id);
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetUserQuery(id);
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllUserQuery();
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
    }
}