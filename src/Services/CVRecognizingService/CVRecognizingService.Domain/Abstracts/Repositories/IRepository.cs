using MongoDB.Bson;

namespace CVRecognizingService.Domain.Abstracts.Repo;

public interface IRepository<T> where T : IEntity
{
    public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
    public Task<T> GetByIdAsync(ObjectId id, CancellationToken cancellationToken);
    public Task<long> AddAsync(T item, CancellationToken cancellationToken);
    public Task<long> AddRangeAsync(List<T> newitems, CancellationToken cancellationToken);
    public Task<long> UpdateAsync(T item, CancellationToken cancellationToken);
    public Task<long> DeleteAsync(ObjectId id, CancellationToken cancellationToken);
    public Task<long> DeleteAsync(T item, CancellationToken cancellationToken);

}
