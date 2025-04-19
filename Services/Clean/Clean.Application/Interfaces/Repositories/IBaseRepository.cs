using System.Linq.Expressions;
using Clean.Application.Common;

namespace Clean.Application.Interfaces.Repositories;

public interface IBaseRepository<TEntity>
    where TEntity : class
{
    Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null);

    Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> predicate);

    Task<TEntity?> GetByIdAsync(string id);

    Task<PagedResult<TEntity>> GetAllPaginatedFiltered(QueryParams queryParams);

    Task AddAsync(TEntity entity);

    Task UpdateAsync(string id, TEntity entity);

    Task<int?> DeleteAsync(int id);

    Task<long> DeleteAsync(Expression<Func<TEntity, bool>> predicate);
}