using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CVRecognizingService.Domain.Abstracts
{
    public abstract class Entity : IEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId(); 
    }
}
