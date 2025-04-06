using Clean.Application.Interfaces;
using Clean.Application.Interfaces.Repositories;
using Clean.Infrastructure.Persistence.Context;
using Clean.Infrastructure.Persistence.Repositories;

namespace Clean.Infrastructure.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly CleanContext _context;

    public UnitOfWork(CleanContext context)
    {
        _context = context;
        Users = new UserRepository(_context);
    }

    public IUserRepository Users { get; }

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}