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