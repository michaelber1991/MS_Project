using Clean.Application.Common;
using Clean.Application.Features.Common.Queries;
using Clean.Application.Features.Users.Commands.CreateUser;
using Clean.Application.Features.Users.Commands.DeleteUser;
using Clean.Application.Features.Users.Queries;
using Clean.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await mediator.Send(new GetUserByIdQuery(id));
        return user != null ? Ok(user) : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParams queryParams)
    {
        var query = new GetAllQuery<User>(queryParams);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        try
        {
            var result = await mediator.Send(command);

            if (result.Success)
                return CreatedAtAction(nameof(CreateUser), new { id = result.Data?.Id }, result.Data);

            return BadRequest(new
            {
                message = result.Message,
                error = result.Errors.Count > 0 ? result.Errors : [result.Message ?? "Unknown error"]
            });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message,
                error = ex.Errors.Select(e => new
                {
                    e.PropertyName, e.ErrorMessage
                }).ToList()
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var result = await mediator.Send(new DeleteUserCommand(id));

            if (result.Success)
                return Ok(new { message = $"User with ID {id} deleted successfully" });

            return BadRequest(new
            {
                message = result.Message,
                error = result.Errors.Count > 0 ? result.Errors.ToArray() : [result.Message ?? "Unknown error"]
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                message = ex.Message,
                error = new[] { "An unexpected error occurred" }
            });
        }
    }
}