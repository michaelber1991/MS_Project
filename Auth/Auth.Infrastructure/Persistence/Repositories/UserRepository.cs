using Auth.Application.Interfaces.Repositories;
using Auth.Domain.Entities;
using Auth.Infrastructure.Persistence.Context;
using Auth.Infrastructure.Persistence.Repositories;

namespace Auth.Infrastructure.Persistence.Repositories;

public class UserRepository(AuthContext context) : BaseRepository<User>(context), IUserRepository
{
}