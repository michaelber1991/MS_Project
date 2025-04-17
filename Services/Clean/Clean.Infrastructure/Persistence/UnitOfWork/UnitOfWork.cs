using Clean.Application.Interfaces;
using Clean.Application.Interfaces.Repositories;
using Clean.Infrastructure.Configurations;
using Clean.Infrastructure.Persistence.Context;
using Clean.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Clean.Infrastructure.Persistence.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly IClientSessionHandle _session;

    public UnitOfWork(IMongoClient mongoClient, IOptions<MongoSettings> settings)
    {
        _session = mongoClient.StartSession();
        var context = new CleanContext(mongoClient, settings.Value.Database);
        Users = new UserRepository(context);
    }

    public IUserRepository Users { get; }

    public async Task<int> CommitAsync()
    {
        try
        {
            // if (_session.IsInTransaction == false)
            // {
            //     _session.StartTransaction();
            // }
            //
            // await _session.CommitTransactionAsync();
            return 1;
        }
        catch
        {
            if (_session.IsInTransaction) await _session.AbortTransactionAsync();
            throw;
        }
    }

    public void Dispose()
    {
        _session.Dispose();
    }
}