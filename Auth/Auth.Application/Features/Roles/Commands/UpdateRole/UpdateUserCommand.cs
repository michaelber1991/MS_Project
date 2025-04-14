using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Clean.Application.Models;
using MediatR;

namespace Auth.Application.Features.Roles.Commands.UpdateRole;

public record UpdateRoleCommand(int Id, string Name, string Description)
    : IRequest<Result<Role>>;

public class UpdateRoleHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateRoleCommand, Result<Role>>
{
    public async Task<Result<Role>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await unitOfWork.Roles.GetByIdAsync(request.Id);
        if (role is null)
            return Result<Role>.Fail(["Application not found"]);

        role.Name = request.Name;
        role.Description = request.Description;

        await unitOfWork.Roles.UpdateAsync(role);
        await unitOfWork.CommitAsync();

        return Result<Role>.Ok(role, "Application updated");
    }
}