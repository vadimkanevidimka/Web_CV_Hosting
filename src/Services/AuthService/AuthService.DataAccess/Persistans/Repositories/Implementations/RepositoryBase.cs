using System.Linq.Expressions;
using AuthService.DataAccess.Persistans.DbContext;
using AuthService.DataAccess.Persistans.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuthService.DataAccess.Persistans.Repositories.Implementations;

public class RepositoryBase<TEntity>
    : IRepository<TEntity>
    where TEntity : class
{
    protected readonly AuthorizationDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    protected RepositoryBase(AuthorizationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetByPredicateAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TEntity item, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(item, cancellationToken);
    }

    public void Delete(TEntity item)
    {
        _dbSet.Remove(item);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}