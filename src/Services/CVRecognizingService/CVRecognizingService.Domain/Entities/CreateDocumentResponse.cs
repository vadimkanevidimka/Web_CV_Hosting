using CVHosting.Shared.Models.Entities.ApplicantProfile;

namespace CVRecognizingService.Domain.Entities
{
    public class CreateDocumentResponse
    {
        public string UserId { get; set; }
        public string DocumentId { get; set; }
        public ApplicantProfile ApplicantProfile { get; set;}
    }
}
