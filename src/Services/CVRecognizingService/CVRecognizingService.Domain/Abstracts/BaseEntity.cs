using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CVRecognizingService.Domain.Abstracts
{
    public class BaseEntity 
    {
        [BsonId] public ObjectId Id { get; private set; } = ObjectId.GenerateNewId();
    }
}
