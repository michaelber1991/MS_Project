using Auth.Application.Interfaces.Repositories;

namespace Auth.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    Task<int> CommitAsync();
}