using CVRecognizingService.Domain.Abstracts;

namespace CVRecognizingService.Domain.Entities;

public class BaseDocument 
    : BaseEntity
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
    public string ContentType { get; private set; } = string.Empty; //Тип файла
    public string FileName { get; private set; } = string.Empty; // Оригинальное имя файла
    public string FilePath { get; private set; } = string.Empty; // Путь к файлу
    public long FileSize { get; private set; }               // Размер файла в байтах
    public DateTime UploadedAt { get; private set; } = DateTime.Now; // Время загрузки файла
    public User? User { get; private set; }
    public DateTime UploadedUntil { get; set; } // Время окончания обработки файла
}
