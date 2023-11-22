using System.Linq.Expressions;
using GatherUs.DAL.Context;
using GatherUs.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace GatherUs.DAL.Repository;

public class DataRepository<TEntity, TKey> : IDataRepository<TEntity, TKey>
    where TEntity : EntityBase<TKey>
{
    protected readonly IDataContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public DataRepository(IDataContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    private IQueryable<TEntity> AggregateIncludes(params Expression<Func<TEntity, object>>[] includes)
    {
        if (includes is not null && includes.Any())
        {
            return includes
                .Aggregate<Expression<Func<TEntity, object>>, IQueryable<TEntity>>(
                    _dbSet,
                    (current, include) => current.Include(include));
        }

        return _dbSet;
    }

    /// <summary>
    /// Retrieves all entities of the generic type.
    /// </summary>
    /// <param name="trackChanges">Track changes.</param>
    /// <param name="includes">List of includes added to the query.</param>
    /// <returns>List of entities.</returns>
    public async Task<List<TEntity>> AllAsync(bool trackChanges = false,
        params Expression<Func<TEntity, object>>[] includes)
    {
        return await HandleSqlExceptionAsync(() =>
        {
            var query = AggregateIncludes(includes);
            return trackChanges ? query.ToListAsync() : query.AsNoTracking().ToListAsync();
        });
    }

    public async Task<List<TEntity>> GetByManyAsync(Expression<Func<TEntity, bool>> filter,
        params Expression<Func<TEntity, object>>[] includes)
    {
        return await HandleSqlExceptionAsync(() =>
        {
            var query = AggregateIncludes(includes);
            return query.Where(filter).ToListAsync();
        });
    }

    public async Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false)
    {
        var query = FormQuery(
            predicate: predicate,
            orderBy: orderBy,
            include: include,
            disableTracking: disableTracking,
            ignoreQueryFilters: ignoreQueryFilters);

        return await query.ToListAsync();
    }

    public async Task<List<TResult>> GetGroupedAsync<TGroupKey, TGroupElement, TResult>(
        Expression<Func<TEntity, TGroupKey>> keySelector,
        Expression<Func<TEntity, TGroupElement>> elementSelector,
        Expression<Func<TGroupKey, IEnumerable<TGroupElement>, TResult>> resultSelector,
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false)
    {
        var query = FormQuery(
            predicate: predicate,
            orderBy: orderBy,
            include: include,
            disableTracking: disableTracking,
            ignoreQueryFilters: ignoreQueryFilters);

        return await query.GroupBy(
            keySelector: keySelector,
            elementSelector: elementSelector,
            resultSelector: resultSelector).ToListAsync();
    }

    public async Task<List<TEntity>> GetLimitedAsync(
        int limit,
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false)
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        if (orderBy != null)
        {
            return await orderBy(query).Take(limit).ToListAsync();
        }
        else
        {
            return await query.Take(limit).ToListAsync();
        }
    }

    public async Task<List<TResult>> GetOffsetPageAsync<TResult>(
        int offset,
        int limit,
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false,
        bool asSplitQuery = false)
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        if (orderBy != null)
        {
            query = orderBy(query);
            query = asSplitQuery ? query.AsSplitQuery() : query;
        }

        return await query.Select(selector).Skip(offset).Take(limit).ToListAsync();
    }

    public async Task<List<TResult>> GetSelectAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false)
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include != null)
        {
            query = include(query);
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        if (orderBy != null)
        {
            return await orderBy(query).Select(selector).ToListAsync();
        }
        else
        {
            return await query.Select(selector).ToListAsync();
        }
    }

    public async Task<TEntity> GetFirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false,
        bool splitQuery = false)
    {
        var query = FormQuery(
            predicate: predicate,
            orderBy: orderBy,
            include: include,
            disableTracking: disableTracking,
            ignoreQueryFilters: ignoreQueryFilters,
            splitQuery: splitQuery);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<int> GetCount(Expression<Func<TEntity, bool>> predicate = null, bool ignoreQueryFilters = false)
    {
        var query = FormQuery(
            predicate: predicate,
            orderBy: null,
            include: null,
            disableTracking: true,
            ignoreQueryFilters: ignoreQueryFilters);

        return await query.CountAsync();
    }

    private IQueryable<TEntity> FormQuery(
        Expression<Func<TEntity, bool>> predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool disableTracking = true,
        bool ignoreQueryFilters = false,
        bool splitQuery = false)
    {
        IQueryable<TEntity> query = _dbSet;

        if (disableTracking)
        {
            query = query.AsNoTracking();
        }

        if (include is not null)
        {
            query = include(query);
        }

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        if (ignoreQueryFilters)
        {
            query = query.IgnoreQueryFilters();
        }

        if (orderBy is not null)
        {
            query = orderBy(query);
        }

        if (splitQuery)
        {
            query = query.AsSplitQuery();
        }

        return query;
    }

    /// <summary>
    /// Inserts or updates the new entity in the DbContext.
    /// </summary>
    /// <param name="entity">Specific object.</param>
    public virtual void AddNew(TEntity entity)
    {
        HandleSqlException(() =>
        {
            entity.Id = default;
            /*if (entity is EntityWithUpdateCreateFields entityWithUpdateCreate)
            {
                DateTime now = DateTime.UtcNow;

                entityWithUpdateCreate.UpdatedAt = now;
                entityWithUpdateCreate.CreatedAt = now;
            }*/

            _dbSet.Add(entity);
        });
    }

    public virtual void AddNewRange(IEnumerable<TEntity> entities)
    {
        HandleSqlException(() =>
        {
            //DateTime now = DateTime.UtcNow;

            foreach (var entity in entities)
            {
                entity.Id = default;

                /*if (entity is EntityWithUpdateCreateFields entityWithUpdateCreate)
                {
                    entityWithUpdateCreate.UpdatedAt = now;
                    entityWithUpdateCreate.CreatedAt = now;
                }*/
            }

            _dbSet.AddRange(entities);
        });
    }

    public async Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> filter, bool ignoreQueryFilters = false,
        params Expression<Func<TEntity, object>>[] includes)
    {
        return await HandleSqlExceptionAsync(() =>
        {
            var query = AggregateIncludes(includes);
            return ignoreQueryFilters
                ? query.IgnoreQueryFilters().FirstOrDefaultAsync(filter)
                : query.FirstOrDefaultAsync(filter);
        });
    }

    public virtual void Update(TEntity entity, bool withTimeUpdate = true)
    {
        HandleSqlException(() =>
        {
            /*if (entity is EntityWithUpdateCreateFields entityWithUpdateCreate && withTimeUpdate)
            {
                DateTime now = DateTime.UtcNow;
                entityWithUpdateCreate.UpdatedAt = now;
            }*/

            _dbSet.Update(entity);
        });
    }

    public void Upsert(TEntity entity)
    {
        HandleSqlException(() =>
        {
            if (entity.Id.Equals(default(TKey)))
            {
                AddNew(entity);
            }
            else
            {
                /*if (entity is EntityWithUpdateCreateFields entityWithUpdateCreate)
                {
                    DateTime now = DateTime.UtcNow;
                    entityWithUpdateCreate.UpdatedAt = now;
                }*/

                Update(entity);
            }
        });
    }

    public void UpsertRange(IEnumerable<TEntity> entities)
    {
        foreach (var entityBase in entities)
        {
            Upsert(entityBase);
        }
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        HandleSqlException(() =>
        {
            /*if (entities?.FirstOrDefault() is EntityWithUpdateCreateFields)
            {
                DateTime now = DateTime.UtcNow;

                foreach (var entity in entities)
                {
                    (entity as EntityWithUpdateCreateFields).UpdatedAt = now;
                }
            }*/

            _dbSet.UpdateRange(entities);
        });
    }

    public async Task Remove(TKey entityId)
    {
        await HandleSqlExceptionAsyncWithoutResult(async () =>
        {
            var entity = await GetByAsync(e => e.Id.Equals(entityId));
            if (entity is not null)
            {
                _dbSet.Remove(entity);
            }
        });
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        HandleSqlException(() => { _dbSet.RemoveRange(entities); });
    }

    public void UpdateEntityOnly(TEntity entity)
    {
        HandleSqlException(() => { _context.Entry(entity).State = EntityState.Modified; });
    }

    public void Attach<T>(T entity)
    {
        _context.Entry(entity).State = EntityState.Unchanged;
    }

    public void Detach<T>(T entity)
    {
        _context.Entry(entity).State = EntityState.Detached;
    }

    public void ClearChangeTracker()
    {
        _context.ChangeTracker.Clear();
    }

    public async Task<int> CountAsync(
        Expression<Func<TEntity, bool>> predicate = null,
        Expression<Func<TEntity, object>> groupingSelector = null)
    {
        return await HandleSqlExceptionAsync(() =>
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            if (groupingSelector is not null)
            {
                query.GroupBy(groupingSelector).CountAsync();
            }

            return query.CountAsync();
        });
    }

    public async Task<int> SumAsync(Expression<Func<TEntity, int>> selector,
        Expression<Func<TEntity, bool>> predicate = null)
    {
        return await HandleSqlExceptionAsync(() =>
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            return query.SumAsync(selector);
        });
    }

    public async Task<decimal> SumAsync(Expression<Func<TEntity, decimal>> selector,
        Expression<Func<TEntity, bool>> predicate = null)
    {
        return await HandleSqlExceptionAsync(() =>
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            return query.SumAsync(selector);
        });
    }

    public async Task<decimal> AvgAsync(Expression<Func<TEntity, decimal>> selector,
        Expression<Func<TEntity, bool>> predicate = null)
    {
        return await HandleSqlExceptionAsync(async () =>
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            try
            {
                return await query.AverageAsync(selector);
            }
            catch (InvalidOperationException)
            {
                return 0m;
            }
        });
    }

    public async Task<decimal> MinAsync(Expression<Func<TEntity, decimal>> selector,
        Expression<Func<TEntity, bool>> predicate = null)
    {
        return await HandleSqlExceptionAsync(async () =>
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate is not null)
            {
                query = query.Where(predicate);
            }

            try
            {
                return await query.MinAsync(selector);
            }
            catch (InvalidOperationException)
            {
                return 0m;
            }
        });
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        if (predicate is null)
        {
            return await _dbSet.AnyAsync(x => true);
        }

        return await _dbSet.AnyAsync(predicate);
    }

    protected static async Task<TResult> HandleSqlExceptionAsync<TResult>(Func<Task<TResult>> func)
    {
        try
        {
            var result = await func();

            return result;
        }
        catch (System.Data.SqlClient.SqlException)
        {
            // add log
            throw;
        }
        catch (Microsoft.Data.SqlClient.SqlException)
        {
            // add log
            throw;
        }
    }

    protected static async Task HandleSqlExceptionAsyncWithoutResult(Func<Task> func)
    {
        try
        {
            await func();
        }
        catch (System.Data.SqlClient.SqlException)
        {
            // add log
            throw;
        }
        catch (Microsoft.Data.SqlClient.SqlException)
        {
            // add log
            throw;
        }
    }

    protected static void HandleSqlException(Action action)
    {
        try
        {
            action();
        }
        catch (System.Data.SqlClient.SqlException)
        {
            throw;
        }
        catch (Microsoft.Data.SqlClient.SqlException)
        {
            throw;
        }
    }
}