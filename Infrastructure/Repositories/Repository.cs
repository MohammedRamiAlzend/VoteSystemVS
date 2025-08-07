using System.Linq.Expressions;
using Domain.Common;
using Domain.Common.Results;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Repositories;

/// <summary>
/// Represents a generic repository for entities of type T.
/// </summary>
/// <typeparam name="T">The type of entity.</typeparam>
public class Repository<T> : IRepository<T> where T : Entity
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    /// <summary>
    /// Initializes a new instance of the <see cref="Repository{T}"/> class.
    /// </summary>
    /// <param name="context">The application's database context.</param>
    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    /// <summary>
    /// Gets an entity by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The identifier of the entity.</param>
    /// <param name="include">A function to include related entities.</param>
    /// <returns>A result containing the entity if found; otherwise, a not found error.</returns>
    public async Task<Result<T?>> GetByIdAsync(int id, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        try
        {
            IQueryable<T> query = _dbSet;
            if (include is not null)
            {
                query = include(query);
            }

            var entity = await query.FirstOrDefaultAsync(e => e.Id == id);
            return entity is not null ? entity : Error.NotFound();
        }
        catch (Exception ex)
        {
            return Error.Failure("GetByIdAsync", ex.Message);
        }
    }

    /// <summary>
    /// Gets all entities asynchronously.
    /// </summary>
    /// <param name="filter">A filter expression to apply.</param>
    /// <param name="include">A function to include related entities.</param>
    /// <returns>A result containing a collection of entities.</returns>
    public async Task<Result<IEnumerable<T>>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        try
        {
            IQueryable<T> query = _dbSet;

            if (filter is not null)
            {
                query = query.Where(filter);
            }

            if (include is not null)
            {
                query = include(query);
            }

            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            return Error.Failure("GetAllAsync", ex.Message);
        }
    }

    /// <summary>
    /// Finds entities based on a predicate asynchronously.
    /// </summary>
    /// <param name="predicate">The predicate to apply.</param>
    /// <param name="include">A function to include related entities.</param>
    /// <returns>A result containing a collection of entities that satisfy the predicate.</returns>
    public async Task<Result<IEnumerable<T>>> FindAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
    {
        try
        {
            IQueryable<T> query = _dbSet;

            if (include is not null)
            {
                query = include(query);
            }

            return await query.Where(predicate).ToListAsync();
        }
        catch (Exception ex)
        {
            return Error.Failure("FindAsync", ex.Message);
        }
    }

    /// <summary>
    /// Adds an entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A result indicating the success of the operation.</returns>
    public async Task<Result<Success>> AddAsync(T entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);
            return Result.Success;
        }
        catch (Exception ex)
        {
            return Error.Failure("AddAsync", ex.Message);
        }
    }

    /// <summary>
    /// Adds a range of entities asynchronously.
    /// </summary>
    /// <param name="entities">The entities to add.</param>
    /// <returns>A result indicating the success of the operation.</returns>
    public async Task<Result<Success>> AddRangeAsync(IEnumerable<T> entities)
    {
        try
        {
            await _dbSet.AddRangeAsync(entities);
            return Result.Success;
        }
        catch (Exception ex)
        {
            return Error.Failure("AddRangeAsync", ex.Message);
        }
    }

    /// <summary>
    /// Removes an entity.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    /// <returns>A result indicating the success of the operation.</returns>
    public async Task<Result<Success>> Remove(T entity)
    {
        try
        {
            _dbSet.Remove(entity);
            return await Task.FromResult(Result.Success);
        }
        catch (Exception ex)
        {
            return Error.Failure("Remove", ex.Message);
        }
    }

    /// <summary>
    /// Removes a range of entities.
    /// </summary>
    /// <param name="entities">The entities to remove.</param>
    /// <returns>A result indicating the success of the operation.</returns>
    public async Task<Result<Success>> RemoveRange(IEnumerable<T> entities)
    {
        try
        {
            _dbSet.RemoveRange(entities);
            return await Task.FromResult(Result.Success);
        }
        catch (Exception ex)
        {
            return Error.Failure("RemoveRange", ex.Message);
        }
    }
}
