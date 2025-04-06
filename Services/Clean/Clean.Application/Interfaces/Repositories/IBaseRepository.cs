using Clean.Application.Common;

namespace Clean.Application.Interfaces.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(int id);
    PagedResult<TEntity> GetAll(QueryParams queryParams);
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task<int?> DeleteAsync(int id);
}