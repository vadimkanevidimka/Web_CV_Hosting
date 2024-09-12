using MongoDB.Bson;

namespace CVRecognizingService.Domain.Entities
{
    public class ProcessedData
    {
        public ProcessedData(ObjectId documentId, string? structuredData, DateTime processedAt, BaseDocument? document)
        {
            DocumentId = documentId;
            StructuredData = structuredData;
            ProcessedAt = processedAt;
            Document = document;
        }

        public ObjectId Id { get; private set; } = ObjectId.GenerateNewId();   // Уникальный идентификатор результата обработки
        public ObjectId DocumentId { get; private set; }             // Идентификатор документа
        public string StructuredData { get; private set; } = string.Empty; // Структурированные данные в формате JSON
        public DateTime ProcessedAt { get; private set; } = DateTime.UtcNow; // Время завершения обработки
        public BaseDocument? Document { get; private set; }
    }
}
