using Auth.Application.Features.Saml.Commands.ProcessSamlLogin;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class SamlController(IMediator mediator) : ControllerBase
{
    [HttpGet("login")]
    public IActionResult Login()
    {
        return Challenge(new AuthenticationProperties
        {
            RedirectUri = "/saml/consume"
        }, "Saml2");
    }

    [HttpPost("consume")]
    public async Task<IActionResult> SamlConsume()
    {
        var result = await HttpContext.AuthenticateAsync("Saml2");
        if (!result.Succeeded) return Unauthorized();

        var command = new ProcessSamlLoginCommand(result.Principal);
        var token = await mediator.Send(command);

        return Redirect($"https://tufrontend.com/auth/callback?token={token}");
    }
}