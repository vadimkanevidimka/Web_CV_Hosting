using MongoDB.Bson;
using CVRecognizingService.Domain.Abstracts;

namespace CVRecognizingService.Domain.Entities;

public class ProcessedData 
    : Entity, IEntity
{
    public ProcessedData(ObjectId documentId, string? structuredData, DateTime processedAt)
    {
        DocumentId = documentId;
        StructuredData = structuredData;
        ProcessedAt = processedAt;
    }
    public ObjectId DocumentId { get; private set; }
    public string StructuredData { get; private set; } = string.Empty;
    public DateTime ProcessedAt { get; private set; } = DateTime.UtcNow;
}
