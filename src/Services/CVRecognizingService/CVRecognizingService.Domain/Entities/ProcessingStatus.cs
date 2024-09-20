using MongoDB.Bson;
using CVRecognizingService.Domain.Enums;
using CVRecognizingService.Domain.Abstracts;

namespace CVRecognizingService.Domain.Entities;

public class ProcessingStatus 
    : Entity, IEntity
{
    public ProcessingStatus(ObjectId documentId, DateTime updatedAt)
    {
        DocumentId = documentId;
        UpdatedAt = updatedAt;
    }

    //[BsonRepresentation(BsonType.String)]
    //public Guid Id { get; private set; } = Guid.NewGuid();
    public ObjectId DocumentId { get; private set; }
    public DocumentState Status { get; set; } = DocumentState.Pending;  // Статус обработки (например, "Pending", "Processing", "Completed", "Error")
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // Время последнего обновления статуса
}
