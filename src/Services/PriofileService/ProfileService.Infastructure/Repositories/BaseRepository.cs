using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProfileService.Domain.Entities;
using ProfileService.Infastructure.DbAccess;
using ProfileService.Infastructure.Extensions;
using System.Linq.Expressions;

namespace ProfileService.Infastructure.Repositories;

public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : IBaseEntity
{
    private readonly ProfileDbContext _dbContext;
    private readonly IMapper _mapper;

    protected BaseRepository(ProfileDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<bool> IsExistingAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Set<TEntity>()
            .AnyAsync(predicate, cancellationToken);
    }

    public async Task<TDestination?> GetAsync<TDestination>(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default, bool exceptIncludes = false, bool trackChanges = true)
    {
        var query = _dbContext.Set<TEntity>()
            .Where(predicate)
            .AsSplitQuery()
            .AsNoTracking();

        IQueryable<TDestination>? projectedQuery;

        projectedQuery = typeof(TDestination) == typeof(TEntity) && exceptIncludes
            ? (IQueryable<TDestination>)query
            : _mapper.ProjectTo<TDestination>(query);

        return await projectedQuery.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<TDestination>> GetAllAsync<TDestination>(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Set<TEntity>().Where(predicate);

        var projectedQuery = typeof(TDestination) == typeof(TEntity)
            ? query as IQueryable<TDestination>
            : query.ProjectTo<TDestination, TEntity>(_mapper);

        return await projectedQuery!
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TDestination>> GetAsync<TDestination>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TDestination>> projection,
        CancellationToken cancellationToken = default,
        bool exceptIncludes = false)
    {
        var query = _dbContext.Set<TEntity>().Where(predicate);

        var projectedQuery = exceptIncludes
            ? query.Select(projection)
            : query.AsNoTracking().Select(projection);

        return await projectedQuery.ToListAsync(cancellationToken);
    }

    public async Task<int> CountByConditionWithDeletedAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Set<TEntity>()
            .IgnoreQueryFilters()
            .Where(predicate)
            .CountAsync(cancellationToken);
    }

    public async Task<int> CountByConditionAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>()
            .Where(predicate)
            .CountAsync(cancellationToken);
    }

    public async Task<IEnumerable<TDestination>> GetAllAsync<TDestination>(CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Set<TEntity>()
            .ProjectTo<TDestination, TEntity>(_mapper)
            .ToListAsync(cancellationToken);
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>()
            .Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task UpdateManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>()
            .UpdateRange(entities);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Set<TEntity>().FirstAsync(e => e.Id == id, cancellationToken);
        _dbContext.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public IQueryable<TDestination> Search<TDestination>(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbContext
            .Set<TEntity>()
            .Where(predicate)
            .AsNoTracking()
            .ProjectTo<TDestination, TEntity>(_mapper);
    }
}