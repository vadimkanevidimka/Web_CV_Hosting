using MongoDB.Bson;
using CVRecognizingService.Domain.Abstracts;

namespace CVRecognizingService.Domain.Entities;

public class ProcessingLog 
    : Entity, IEntity
{
    public ObjectId DocumentId { get; set; }      
    public string LogMessage { get; set; } = string.Empty;
    public DateTime LoggedAt { get; set; } = DateTime.UtcNow;
}
