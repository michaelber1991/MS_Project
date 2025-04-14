using Auth.Application.Common;
using Auth.Application.Features.Common.Queries;
using Auth.Application.Features.Roles.Commands.CreateRole;
using Auth.Application.Features.Roles.Commands.DeleteRole;
using Auth.Application.Features.Roles.Commands.UpdateRole;
using Auth.Application.Features.Roles.Queries;
using Auth.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class RolesController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var role = await mediator.Send(new GetRoleByIdQuery(id));
        return role != null ? Ok(role) : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParams queryParams)
    {
        var query = new GetAllQuery<Role>(queryParams);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand command)
    {
        try
        {
            var result = await mediator.Send(command);

            if (result.Succeeded)
                return CreatedAtAction(nameof(CreateRole), new { id = result.Data?.Id }, result.Data);

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
    public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleCommand command)
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
    public async Task<IActionResult> DeleteRole(int id)
    {
        try
        {
            var result = await mediator.Send(new DeleteRoleCommand(id));

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
}