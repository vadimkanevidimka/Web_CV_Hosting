using ProfileService.Domain.Entities;
namespace ProfileService.Infastructure.Repositories;

using System.Linq.Expressions;

public interface IBaseRepository<TEntity> where TEntity : class, IBaseEntity
{
    Task<bool> IsExistingAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default, bool exceptIncludes = false, bool trackChanges = true);

    Task<IEnumerable<TDestination>> GetAllAsync<TDestination>(CancellationToken cancellationToken = default);

    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    IQueryable<TDestination> Search<TDestination>(Expression<Func<TEntity, bool>> predicate);

    Task UpdateManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task<IEnumerable<TDestination>> GetAllAsync<TDestination>(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task<int> CountByConditionWithDeletedAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default);

    Task<int> CountByConditionAsync(Expression<Func<TEntity, bool>> condition, CancellationToken cancellationToken = default);

    Task<IEnumerable<TDestination>> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default, bool exceptIncludes = false);
}