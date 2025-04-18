using Auth.Application.Features.Auth.Commands.LoginWithPassword;
using Auth.Application.Features.Auth.Commands.ProcessSamlLogin;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(IMediator mediator, IConfiguration configuration) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration;

    [HttpPost("credentials/login")]
    public async Task<IActionResult> Login([FromBody] LoginWithPasswordCommand request)
    {
        var token = await mediator.Send(request);

        if (token is null)
            return Unauthorized();

        return Ok(new { token });
    }

    [HttpGet("saml/login")]
    public IActionResult Login()
    {
        return Challenge(new AuthenticationProperties
        {
            RedirectUri = Url.Action("SamlConsume", "Auth", null, Request.Scheme)
        }, "Saml2");
    }

    [HttpPost("saml/consume")]
    public async Task<IActionResult> SamlConsume()
    {
        var result = await HttpContext.AuthenticateAsync("Saml2");
        if (!result.Succeeded) return Unauthorized();

        var command = new ProcessSamlLoginCommand(result.Principal);
        var token = await mediator.Send(command);

        var frontendRedirect = _configuration["Frontend:AuthCallbackUrl"];
        return Redirect($"{frontendRedirect}?token={token}");
    }
}