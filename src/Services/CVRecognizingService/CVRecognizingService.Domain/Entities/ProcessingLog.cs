using MongoDB.Bson;
using CVRecognizingService.Domain.Abstracts;

namespace CVRecognizingService.Domain.Entities;

public class ProcessingLog 
    : Entity, IEntity
{
    //[BsonRepresentation(BsonType.String)]
    //public Guid Id { get; private set; } = Guid.NewGuid();
    public ObjectId DocumentId { get; set; }      
    public string LogMessage { get; set; } = string.Empty;
    public DateTime LoggedAt { get; set; } = DateTime.UtcNow;
}
