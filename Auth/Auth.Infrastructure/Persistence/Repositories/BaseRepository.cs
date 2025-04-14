using System.Linq.Expressions;
using Auth.Application.Common;
using Auth.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Auth.Infrastructure.Persistence.Repositories;

public class BaseRepository<TEntity>(AuthContext context)
    where TEntity : class
{
    public async Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();

        foreach (var include in includes) query = query.Include(include);

        if (predicate != null) query = query.Where(predicate);

        return await query.ToListAsync();
    }

    public async Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();

        if (include != null)
            query = include(query);

        return await query.FirstOrDefaultAsync(predicate);
    }

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await context.Set<TEntity>().FindAsync(id);
    }

    public PagedResult<TEntity> GetAllPaginatedFiltered(QueryParams queryParams)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();

        if (queryParams.Filters != null)
        {
            var filters = JsonConvert.DeserializeObject<List<Filter>>(queryParams.Filters);
            if (filters != null)
                foreach (var filter in filters)
                    query = ApplyFilter(query, filter);
        }

        if (queryParams.Orders != null)
        {
            var orders = JsonConvert.DeserializeObject<List<Order>>(queryParams.Orders);
            if (orders != null)
                foreach (var order in orders)
                    query = ApplyOrder(query, order);
        }

        var totalCount = query.Count();

        if (queryParams is { PageNumber: not null, PageSize: not null })
            query = query.Skip((queryParams.PageNumber.Value - 1) * queryParams.PageSize.Value)
                .Take(queryParams.PageSize.Value);

        var data = query.ToList();
        return new PagedResult<TEntity>(data, totalCount);
    }

    public async Task AddAsync(TEntity entity)
    {
        await context.Set<TEntity>().AddAsync(entity);
    }

    public Task UpdateAsync(TEntity entity)
    {
        context.Set<TEntity>().Update(entity);
        return Task.CompletedTask;
    }

    public async Task<int?> DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            context.Set<TEntity>().Remove(entity);
            return id;
        }

        return null;
    }

    public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var entities = await context.Set<TEntity>().Where(predicate).ToListAsync();
        if (entities.Count == 0) return 0;

        context.Set<TEntity>().RemoveRange(entities);
        return entities.Count;
    }

    #region Private Methods

    private IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> query, Filter filter)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "x");
        var property = Expression.Property(parameter, filter.Property);
        var constant = Expression.Constant(filter.Value);
        Expression? condition = null;

        switch (filter.Type.ToLower())
        {
            case "equals":
                condition = Expression.Equal(property, constant);
                break;
            case "contains":
                condition = Expression.Call(property, "Contains", null, constant);
                break;
        }

        if (condition != null)
        {
            var lambda = Expression.Lambda<Func<TEntity, bool>>(condition, parameter);
            return query.Where(lambda);
        }

        return null!;
    }

    private IQueryable<TEntity> ApplyOrder(IQueryable<TEntity> query, Order order)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "x");
        var property = Expression.Property(parameter, order.Property);
        var lambda = Expression.Lambda(property, parameter);

        var methodName = order.Ascending ? "OrderBy" : "OrderByDescending";
        var resultExp = Expression.Call(
            typeof(Queryable),
            methodName,
            [query.ElementType, property.Type],
            query.Expression,
            lambda
        );

        return query.Provider.CreateQuery<TEntity>(resultExp);
    }

    #endregion
}