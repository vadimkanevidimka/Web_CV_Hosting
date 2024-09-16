using CVRecognizingService.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace CVRecognizingService.Domain.DTOs.Outgoing
{
    public class RootDocumentDto
    {
        public BaseDocument Document { get; set; }
        public ProcessedData ProcessedData { get; set; }
        public ProcessingStatus ProcessingStatus { get; set; }
        public ProcessingLog ProcessingLog { get; set; }
    }
}
