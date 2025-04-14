using Auth.Application.Interfaces;
using Auth.Application.Interfaces.Repositories;
using Auth.Infrastructure.Persistence.Context;
using Auth.Infrastructure.Persistence.Repositories;

namespace Auth.Infrastructure.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly AuthContext _context;

    public UnitOfWork(AuthContext context)
    {
        _context = context;
        Users = new UserRepository(_context);
        Applications = new ApplicationRepository(_context);
        Roles = new RoleRepository(_context);
    }

    public IUserRepository Users { get; }
    public IApplicationRepository Applications { get; }
    public IRoleRepository Roles { get; }

    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}