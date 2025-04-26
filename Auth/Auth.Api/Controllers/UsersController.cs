using Auth.Application.Common;
using Auth.Application.Features.Common.Queries;
using Auth.Application.Features.Users.Commands.AssignUserToApplication;
using Auth.Application.Features.Users.Commands.AssignUserToRole;
using Auth.Application.Features.Users.Commands.CreateUser;
using Auth.Application.Features.Users.Commands.DeleteUser;
using Auth.Application.Features.Users.Commands.UpdateUser;
using Auth.Application.Features.Users.Queries;
using Auth.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

// [Authorize(Roles = "Admin")]
[Route("[controller]")]
[ApiController]
public class UsersController(IMediator mediator) : ControllerBase
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

            if (result.Succeeded)
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

    [HttpPatch]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
    {
        try
        {
            var result = await mediator.Send(command);

            if (result.Succeeded)
                return Ok(result.Data);

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
                    e.PropertyName,
                    e.ErrorMessage
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

            if (result.Succeeded)
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

    [HttpPost("{userId}/applications/{applicationId}/assign")]
    public async Task<IActionResult> AssignUserToApplication(int userId, int applicationId)
    {
        var result = await mediator.Send(new AssignUserToApplicationCommand(userId, applicationId));
        if (!result.Succeeded) return BadRequest(result.Message);

        return Ok(result.Message);
    }

    [HttpPost("{userId}/applications/{applicationId}/roles/{roleId}/assign")]
    public async Task<IActionResult> AssignUserToRole(int userId, int applicationId, int roleId)
    {
        var result = await mediator.Send(new AssignUserToRoleCommand(userId, applicationId, roleId));
        if (!result.Succeeded) return BadRequest(result.Message);

        return Ok(result.Message);
    }
}