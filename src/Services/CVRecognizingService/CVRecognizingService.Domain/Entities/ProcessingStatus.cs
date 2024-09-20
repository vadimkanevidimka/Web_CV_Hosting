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
    public ObjectId DocumentId { get; private set; }
    public DocumentState Status { get; set; } = DocumentState.Pending;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
