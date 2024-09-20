using MongoDB.Bson;

namespace CVRecognizingService.Domain.Abstracts.Repo;

public interface IRepository<T> where T : IEntity
{
    public Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken);
    public Task<T> Get(ObjectId id, CancellationToken cancellationToken);
    public Task<long> Add(T item, CancellationToken cancellationToken);
    public Task<long> Add(List<T> newitems, CancellationToken cancellationToken);
    public Task<long> Update(T item, CancellationToken cancellationToken);
    public Task<long> Delete(ObjectId id, CancellationToken cancellationToken);
    public Task<long> Delete(T item, CancellationToken cancellationToken);

}
