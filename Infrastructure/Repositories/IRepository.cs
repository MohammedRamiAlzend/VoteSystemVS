using System.Linq.Expressions;
using Domain.Common;
using Domain.Common.Results;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Repositories;

public interface IRepository<T> where T : Entity
{
    Task<Result<T?>> GetByIdAsync(int id, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    Task<Result<IEnumerable<T>>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    Task<Result<IEnumerable<T>>> FindAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
    Task<Result<Success>> AddAsync(T entity);
    Task<Result<Success>> AddRangeAsync(IEnumerable<T> entities);
    Task<Result<Success>> Remove(T entity);
    Task<Result<Success>> UpdateAsync(T entity);
    Task<Result<Success>> UpdateRangeAsync(IEnumerable<T> entities);
    Task<Result<Success>> RemoveRange(IEnumerable<T> entities);
    Task<Result<Success>> AnyAsync(Expression<Func<T, bool>>? filter = null);
}
