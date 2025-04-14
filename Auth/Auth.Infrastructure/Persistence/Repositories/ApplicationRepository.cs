using Auth.Application.Interfaces.Repositories;
using Auth.Infrastructure.Persistence.Context;

namespace Auth.Infrastructure.Persistence.Repositories;

public class ApplicationRepository(AuthContext context)
    : BaseRepository<Domain.Entities.Application>(context), IApplicationRepository
{
}