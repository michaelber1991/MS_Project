using Clean.Application.Interfaces.Repositories;

namespace Clean.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    Task<int> CommitAsync();
}