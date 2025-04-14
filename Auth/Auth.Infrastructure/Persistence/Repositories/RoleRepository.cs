using Auth.Application.Interfaces.Repositories;
using Auth.Domain.Entities;
using Auth.Infrastructure.Persistence.Context;

namespace Auth.Infrastructure.Persistence.Repositories;

public class RoleRepository(AuthContext context) : BaseRepository<Role>(context), IRoleRepository
{
}