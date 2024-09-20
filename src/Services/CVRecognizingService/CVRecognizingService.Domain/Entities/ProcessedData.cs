using MongoDB.Bson;
using CVRecognizingService.Domain.Abstracts;

namespace CVRecognizingService.Domain.Entities
{
    public class ProcessedData : BaseEntity
    {
        public ProcessedData(ObjectId documentId, string? structuredData, DateTime processedAt)
        {
            DocumentId = documentId;
            StructuredData = structuredData;
            ProcessedAt = processedAt;
        }
        public ObjectId DocumentId { get; private set; }             // Идентификатор документа
        public string StructuredData { get; private set; } = string.Empty; // Структурированные данные в формате JSON
        public DateTime ProcessedAt { get; private set; } = DateTime.UtcNow; // Время завершения обработки
    }
}
