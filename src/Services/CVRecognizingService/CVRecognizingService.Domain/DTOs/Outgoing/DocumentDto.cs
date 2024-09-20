
using CVRecognizingService.Domain.Entities;

namespace CVRecognizingService.Domain.DTOs.Outgoing
{
    public class DocumentDto
    {
        public User User { get; set; }
        public Document Document { get; set; }
        public ProcessedData ProcessedData { get; set; }
        public ProcessingLog ProcessingLog { get; set; }
        public ProcessingStatus ProcessingStatus { get; set; }
    }
}
