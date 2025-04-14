using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Clean.Application.Models;
using MediatR;

namespace Auth.Application.Features.Users.Commands.AssignUserToRole;

public record AssignUserToRoleCommand(int UserId, int ApplicationId, int RoleId) : IRequest<Result<string>>;

public class AssignUserToRoleHandler(IUnitOfWork unitOfWork) : IRequestHandler<AssignUserToRoleCommand, Result<string>>
{
    public async Task<Result<string>> Handle(AssignUserToRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await unitOfWork.Users.GetByIdAsync(request.UserId);
        if (user == null) return Result<string>.Fail(["User not found"]);

        var application = await unitOfWork.Applications.GetByIdAsync(request.ApplicationId);
        if (application == null) return Result<string>.Fail(["Application not found"]);

        var role = await unitOfWork.Roles.GetByIdAsync(request.RoleId);
        if (role == null) return Result<string>.Fail(["Role not found"]);

        var userRole = new UserRole
        {
            UserId = request.UserId,
            ApplicationId = request.ApplicationId,
            RoleId = request.RoleId,
            CreatedAt = DateTime.UtcNow
        };

        user.UserRoles.Add(userRole);
        await unitOfWork.CommitAsync();

        return Result<string>.Ok("User assigned to role");
    }
}