using Auth.Application.Interfaces;
using Auth.Application.Interfaces.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Auth.Application.Features.Auth.Commands.LoginWithPassword;

public record LoginWithPasswordCommand(string Username, string Password) : IRequest<string>;

public class LoginWithPasswordCommandHandler(
    IUnitOfWork unitOfWork,
    IJwtGeneratorService jwtGenerator,
    IConfiguration configuration)
    : IRequestHandler<LoginWithPasswordCommand, string>
{
    public async Task<string> Handle(LoginWithPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetAsync(
            u => u.Username == request.Username,
            query => query.Include(u => u.UserApplications)
                .ThenInclude(ua => ua.Application)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role));

        var applications = user?.UserApplications
            .Select(ur => ur.Application.Name)
            .ToList();

        var expectedAppName = configuration["ApplicationName"];

        if (user != null && !user.VerifyPassword(request.Password) && applications != null &&
            applications.Contains(expectedAppName!))
            return null;

        var roles = user?.UserRoles
            .Select(ur => ur.Role.Name)
            .ToList();

        return jwtGenerator.GenerateToken(user, roles);
    }
}