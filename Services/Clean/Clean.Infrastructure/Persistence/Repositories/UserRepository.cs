using Clean.Application.Interfaces.Repositories;
using Clean.Domain.Entities;
using Clean.Infrastructure.Persistence.Context;

namespace Clean.Infrastructure.Persistence.Repositories;

public class UserRepository(CleanContext context) : BaseRepository<User>(context.Users), IUserRepository
{
}