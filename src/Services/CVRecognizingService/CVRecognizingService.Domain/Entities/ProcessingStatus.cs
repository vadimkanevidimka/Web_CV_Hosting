using MongoDB.Bson;
using CVRecognizingService.Domain.Abstracts;

namespace CVRecognizingService.Domain.Entities
{
    public class ProcessingStatus : BaseEntity
    {
        public ProcessingStatus(ObjectId documentId, string status, DateTime updatedAt)
        {
            DocumentId = documentId;
            Status = status;
            UpdatedAt = updatedAt;
        }
        public ObjectId DocumentId { get; private set; }             // Идентификатор документа
        public string Status { get; set; } = "Pending";  // Статус обработки (например, "Pending", "Processing", "Completed", "Error")
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // Время последнего обновления статуса
    }
}
