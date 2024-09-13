using MongoDB.Bson;
using CVRecognizingService.Domain.Abstracts;

namespace CVRecognizingService.Domain.Entities
{
    public class ProcessingLog : BaseEntity
    {
        public ObjectId DocumentId { get; set; }             // Идентификатор документа
        public string LogMessage { get; set; } = string.Empty; // Сообщение лога (ошибка или результат)
        public DateTime LoggedAt { get; set; } = DateTime.UtcNow; // Время записи лога
    }
}
