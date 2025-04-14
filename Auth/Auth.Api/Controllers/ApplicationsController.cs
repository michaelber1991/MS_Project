using Auth.Application.Common;
using Auth.Application.Features.Applications.Commands.CreateApplication;
using Auth.Application.Features.Applications.Commands.DeleteApplication;
using Auth.Application.Features.Applications.Commands.UpdateApplication;
using Auth.Application.Features.Applications.Queries;
using Auth.Application.Features.Common.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class ApplicationsController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var application = await mediator.Send(new GetApplicationByIdQuery(id));
        return application != null ? Ok(application) : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryParams queryParams)
    {
        var query = new GetAllQuery<Domain.Entities.Application>(queryParams);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateApplication([FromBody] CreateApplicationCommand command)
    {
        try
        {
            var result = await mediator.Send(command);

            if (result.Succeeded)
                return CreatedAtAction(nameof(CreateApplication), new { id = result.Data?.Id }, result.Data);

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
    public async Task<IActionResult> UpdateApplication([FromBody] UpdateApplicationCommand command)
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
    public async Task<IActionResult> DeleteApplication(int id)
    {
        try
        {
            var result = await mediator.Send(new DeleteApplicationCommand(id));

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