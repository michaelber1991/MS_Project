using Auth.Application.Features.Credentials.Commands.LoginWithPassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class CredentialsController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginWithPasswordCommand request)
    {
        var token = await mediator.Send(request);

        if (token is null)
            return Unauthorized();

        return Ok(new { token });
    }
}