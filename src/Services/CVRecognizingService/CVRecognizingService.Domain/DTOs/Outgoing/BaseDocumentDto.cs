using CVRecognizingService.Domain.Entities;

namespace CVRecognizingService.Domain.DTOs.Outgoing
{
    public class BaseDocumentDto
    {
        public string Id { get; set; }
        public string ContentType { get; private set; } = string.Empty;
        public string FileName { get; private set; } = string.Empty;
        public string FilePath { get; private set; } = string.Empty;
        public long FileSize { get; private set; }
        public DateTime UploadedAt { get; private set; } = DateTime.Now;
        public string UserId { get; private set; }
        public DateTime UploadedUntil { get; set; }
    }
}
