using MongoDB.Bson;

namespace CVRecognizingService.Domain.Entities
{
    public class BaseDocument
    {
        public BaseDocument(string contentType, string fileName, string filePath, long fileSize, DateTime uploadedAt, User? user)
        {
            ContentType = contentType;
            FileName = fileName;
            FilePath = filePath;
            FileSize = fileSize;
            UploadedAt = uploadedAt;
            User = user;
        }

        public ObjectId Id { get; private set; } = ObjectId.GenerateNewId();   // Уникальный идентификатор документа
        public string ContentType { get; private set; } = string.Empty; //Тип файла
        public string FileName { get; private set; } = string.Empty; // Оригинальное имя файла
        public string FilePath { get; private set; } = string.Empty; // Путь к файлу
        public long FileSize { get; private set; }               // Размер файла в байтах
        public DateTime UploadedAt { get; private set; } = DateTime.Now; // Время загрузки файла
        public DateTime UploadedUntil { get; set; }
        public User? User { get; set; }

        // Навигационные свойства для связи с результатом обработки
        public ProcessedData ProcessedData { get; set; }
        public ProcessingStatus ProcessingStatuses { get; set; }
        public ProcessingLog ProcessingLogs { get; set; }
    }
}
