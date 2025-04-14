using System.Linq.Expressions;
using Auth.Application.Common;

namespace Auth.Application.Interfaces.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        params Expression<Func<TEntity, object>>[] includes);

    Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);

    Task<TEntity?> GetByIdAsync(int id);
    PagedResult<TEntity> GetAllPaginatedFiltered(QueryParams queryParams);
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task<int?> DeleteAsync(int id);
    Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate);
}