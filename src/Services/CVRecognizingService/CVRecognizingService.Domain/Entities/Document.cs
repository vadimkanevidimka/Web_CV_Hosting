using CVRecognizingService.Domain.Abstracts;

namespace CVRecognizingService.Domain.Entities;

public class Document 
    : Entity, IEntity
{
    public Document(
        string contentType,
        string fileName,
        string filePath,
        long fileSize,
        DateTime uploadedAt,
        User? user)
    {
        ContentType = contentType;
        FileName = fileName;
        FilePath = filePath;
        FileSize = fileSize;
        UploadedAt = uploadedAt;
        User = user;
    }
    public string ContentType { get; private set; } = string.Empty;
    public string FileName { get; private set; } = string.Empty;
    public string FilePath { get; private set; } = string.Empty;
    public long FileSize { get; private set; }
    public DateTime UploadedAt { get; private set; } = DateTime.Now;
    public User? User { get; private set; }
    public DateTime UploadedUntil { get; set; }
}
