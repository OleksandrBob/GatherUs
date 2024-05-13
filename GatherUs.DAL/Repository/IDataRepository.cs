using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace GatherUs.DAL.Repository;

public interface IDataRepository<TEntity, TKey>
{
    Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> filter, bool ignoreQueryFilters = false,
        params Expression<Func<TEntity, object>>[] includes);

    public Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false);

    public Task<List<TResult>> GetSelectAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false);

    public Task<TEntity> GetFirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false,
        bool splitQuery = true);

    void AddNew(TEntity entity);

    void Update(TEntity entity, bool withTimeUpdate = true);

    Task Remove(TKey entityId);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
}

public interface IDataRepository<TEntity> : IDataRepository<TEntity, int>
{
}