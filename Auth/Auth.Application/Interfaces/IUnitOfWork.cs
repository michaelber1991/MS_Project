using Auth.Application.Interfaces.Repositories;

namespace Auth.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IApplicationRepository Applications { get; }
    IRoleRepository Roles { get; }
    Task<int> CommitAsync();
}