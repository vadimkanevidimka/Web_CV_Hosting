using MongoDB.Bson;
namespace CVRecognizingService.Domain.Abstracts;
public interface IEntity
{
    public ObjectId Id { get; set; }
}
