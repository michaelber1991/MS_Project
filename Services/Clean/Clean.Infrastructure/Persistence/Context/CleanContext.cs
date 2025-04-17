using Clean.Domain.Entities;
using MongoDB.Driver;

namespace Clean.Infrastructure.Persistence.Context;

public class CleanContext(IMongoClient mongoClient, string databaseName)
{
    private readonly IMongoDatabase _database = mongoClient.GetDatabase(databaseName);
    public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
}