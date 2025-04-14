using Auth.Application.Interfaces.Repositories;
using Auth.Domain.Entities;
using MediatR;

namespace Auth.Application.Features.Roles.Queries;

public record GetRoleByIdQuery(int Id) : IRequest<Role?>;

public class GetRoleByIdHandler(IRoleRepository roleRepository) : IRequestHandler<GetRoleByIdQuery, Role?>
{
    public async Task<Role?> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        return await roleRepository.GetByIdAsync(request.Id);
    }
}