using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Clean.Application.Models;
using MediatR;

namespace Auth.Application.Features.Roles.Commands.CreateRole;

public record CreateRoleCommand(string Name, string Description)
    : IRequest<Result<Role>>;

public class CreateRoleHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateRoleCommand, Result<Role>>
{
    public async Task<Result<Role>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = new Role
        {
            Name = request.Name,
            Description = request.Description
        };

        await unitOfWork.Roles.AddAsync(role);
        await unitOfWork.CommitAsync();

        return Result<Role>.Ok(role, "User created");
    }
}