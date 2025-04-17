using System.Linq.Expressions;
using Clean.Application.Common;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Clean.Infrastructure.Persistence.Repositories;

public class BaseRepository<TEntity>(IMongoCollection<TEntity> collection)
    where TEntity : class
{
    public async Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null)
    {
        var filter = predicate != null ? Builders<TEntity>.Filter.Where(predicate) : Builders<TEntity>.Filter.Empty;
        return await collection.Find(filter).ToListAsync();
    }

    public async Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> predicate)
    {
        return await collection.Find(predicate).FirstOrDefaultAsync();
    }

    public async Task<TEntity?> GetByIdAsync(string id)
    {
        var objectId = ObjectId.Parse(id);
        var filter = Builders<TEntity>.Filter.Eq("_id", objectId);
        return await collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        var filter = Builders<TEntity>.Filter.Eq("Id", id);
        return await collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<PagedResult<TEntity>> GetAllPaginatedFiltered(QueryParams queryParams)
    {
        var filter = Builders<TEntity>.Filter.Empty;

        if (queryParams.Filters != null)
        {
            var filters = JsonConvert.DeserializeObject<List<Filter>>(queryParams.Filters);
            var filterDefinitions = new List<FilterDefinition<TEntity>>();
            if (filters != null)
                foreach (var f in filters)
                    if (f.Type.ToLower() == "equals")
                        filterDefinitions.Add(Builders<TEntity>.Filter.Eq(f.Property, f.Value));
                    else if (f.Type.ToLower() == "contains")
                        filterDefinitions.Add(Builders<TEntity>.Filter.Regex(f.Property,
                            new BsonRegularExpression(f.Value.ToString(), "i")));

            if (filterDefinitions.Any())
                filter = Builders<TEntity>.Filter.And(filterDefinitions);
        }

        var findOptions = collection.Find(filter);

        if (queryParams.Orders != null)
        {
            var orders = JsonConvert.DeserializeObject<List<Order>>(queryParams.Orders);
            if (orders != null)
                foreach (var order in orders)
                {
                    var sort = order.Ascending
                        ? Builders<TEntity>.Sort.Ascending(order.Property)
                        : Builders<TEntity>.Sort.Descending(order.Property);
                    findOptions = findOptions.Sort(sort);
                }
        }

        var totalCount = await findOptions.CountDocumentsAsync();

        if (queryParams is { PageNumber: not null, PageSize: not null })
            findOptions = findOptions
                .Skip((queryParams.PageNumber.Value - 1) * queryParams.PageSize.Value)
                .Limit(queryParams.PageSize.Value);

        var data = await findOptions.ToListAsync();
        return new PagedResult<TEntity>(data, (int)totalCount);
    }

    public async Task AddAsync(TEntity entity)
    {
        await collection.InsertOneAsync(entity);
    }

    public async Task UpdateAsync(string id, TEntity entity)
    {
        var objectId = ObjectId.Parse(id);
        var filter = Builders<TEntity>.Filter.Eq("_id", objectId);
        await collection.ReplaceOneAsync(filter, entity);
    }

    public async Task<string?> DeleteAsync(string id)
    {
        var objectId = ObjectId.Parse(id);
        var result = await collection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", objectId));
        return result.DeletedCount > 0 ? id : null;
    }

    public async Task<long> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var result = await collection.DeleteManyAsync(predicate);
        return result.DeletedCount;
    }

    public async Task<int?> DeleteAsync(int id)
    {
        var filter = Builders<TEntity>.Filter.Eq("Id", id);
        var result = await collection.DeleteOneAsync(filter);
        return result.DeletedCount > 0 ? id : null;
    }
}