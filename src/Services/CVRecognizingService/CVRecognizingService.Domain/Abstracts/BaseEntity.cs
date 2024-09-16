using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CVRecognizingService.Domain.Abstracts
{
    public class BaseEntity 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    }
}
