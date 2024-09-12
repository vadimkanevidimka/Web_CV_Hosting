using MongoDB.Bson;

namespace CVRecognizingService.Domain.Entities
{
    public class ProcessingStatus
    {
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();   // Уникальный идентификатор статуса
        public ObjectId DocumentId { get; set; }             // Идентификатор документа
        public string Status { get; set; } = "Pending";  // Статус обработки (например, "Pending", "Processing", "Completed", "Error")
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow; // Время последнего обновления статуса
        public BaseDocument? Document { get; set; }
    }
}
