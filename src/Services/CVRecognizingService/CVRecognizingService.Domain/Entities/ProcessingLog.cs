using MongoDB.Bson;

namespace CVRecognizingService.Domain.Entities
{
    public class ProcessingLog
    {
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();   // Уникальный идентификатор лога
        public ObjectId DocumentId { get; set; }             // Идентификатор документа
        public string LogMessage { get; set; } = string.Empty; // Сообщение лога (ошибка или результат)
        public DateTime LoggedAt { get; set; } = DateTime.UtcNow; // Время записи лога
        public BaseDocument? Document { get; set; }
    }
}
