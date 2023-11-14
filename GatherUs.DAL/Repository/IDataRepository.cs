using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace GatherUs.DAL.Repository;

public interface IDataRepository<TEntity, TKey>
{
    Task<List<TEntity>> AllAsync(bool trackChanges = false, params Expression<Func<TEntity, object>>[] includes);

    Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> filter, bool ignoreQueryFilters = false,
        params Expression<Func<TEntity, object>>[] includes);

    Task<List<TEntity>> GetByManyAsync(Expression<Func<TEntity, bool>> filter,
        params Expression<Func<TEntity, object>>[] includes);

    public Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false);

    public Task<List<TResult>> GetGroupedAsync<TGroupKey, TGroupElement, TResult>(
        Expression<Func<TEntity, TGroupKey>> keySelector,
        Expression<Func<TEntity, TGroupElement>> elementSelector,
        Expression<Func<TGroupKey, IEnumerable<TGroupElement>, TResult>> resultSelector,
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false);

    /// <summary>
    /// Returns limited amount of entities.
    /// Used for token based pagination.
    /// </summary>
    /// <param name="limit">The amount of entities to select.
    /// Stands for page size for pagination queries.</param>
    /// <param name="predicate">Predicate used for filtering.
    /// For token based pagination(token is unique field of the entity) predicate should
    /// get entities with the field value more(or less for previous pages) than token value.</param>
    /// <param name="orderBy">For token based pagination should be ordered by token field value.
    /// For next pages - ascending, for previous pages - descending(usually).</param>
    /// <returns>List of entities.</returns>.
    public Task<List<TEntity>> GetLimitedAsync(
        int limit,
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

    Task<List<TResult>> GetOffsetPageAsync<TResult>(
        int offset,
        int limit,
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false,
        bool asSplitQuery = false);

    public Task<int> GetCount(Expression<Func<TEntity, bool>> predicate = null, bool ignoreQueryFilters = false);

    void AddNew(TEntity entity);

    void AddNewRange(IEnumerable<TEntity> entities);

    void Update(TEntity entity, bool withTimeUpdate = true);

    void Upsert(TEntity entity);
    
    void UpsertRange(IEnumerable<TEntity> entity);

    void UpdateEntityOnly(TEntity entity);

    void UpdateRange(IEnumerable<TEntity> entities);

    Task Remove(TKey entityId);

    void RemoveRange(IEnumerable<TEntity> entities);

    void Attach<T>(T entity);

    void Detach<T>(T entity);

    void ClearChangeTracker();

    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null,
        Expression<Func<TEntity, object>> groupingSelector = null);

    Task<int> SumAsync(Expression<Func<TEntity, int>> selector, Expression<Func<TEntity, bool>> predicate = null);

    Task<decimal> SumAsync(Expression<Func<TEntity, decimal>> selector,
        Expression<Func<TEntity, bool>> predicate = null);

    Task<decimal> AvgAsync(Expression<Func<TEntity, decimal>> selector,
        Expression<Func<TEntity, bool>> predicate = null);

    Task<decimal> MinAsync(Expression<Func<TEntity, decimal>> selector,
        Expression<Func<TEntity, bool>> predicate = null);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
}

public interface IDataRepository<TEntity> : IDataRepository<TEntity, int>
{
}