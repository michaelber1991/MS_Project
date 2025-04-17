using System.Security.Claims;
using Auth.Application.Interfaces;
using Auth.Application.Interfaces.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Auth.Application.Features.Auth.Commands.ProcessSamlLogin;

public record ProcessSamlLoginCommand(ClaimsPrincipal Principal) : IRequest<string>;

public class ProcessSamlLoginHandler(IUnitOfWork unitOfWork, IJwtGeneratorService jwtGenerator)
    : IRequestHandler<ProcessSamlLoginCommand, string>
{
    public async Task<string> Handle(ProcessSamlLoginCommand request, CancellationToken cancellationToken)
    {
        var principal = request.Principal;
        var nameId = principal.FindFirst("NameID")?.Value;

        if (string.IsNullOrEmpty(nameId))
            throw new UnauthorizedAccessException("SAML assertion missing NameID");


        var user = await unitOfWork.Users.GetAsync(
            u => u.Username == nameId,
            query => query.Include(u => u.UserApplications)
                .ThenInclude(ua => ua.Application)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
        );


        var roles = user.UserRoles!
            .Select(ur => ur.Role.Name)
            .ToList();

        var token = jwtGenerator.GenerateToken(user, roles);
        return token;
    }
}