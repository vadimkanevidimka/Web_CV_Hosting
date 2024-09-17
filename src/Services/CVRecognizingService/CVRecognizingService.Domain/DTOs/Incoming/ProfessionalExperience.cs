namespace CVRecognizingService.Domain.DTOs.Incoming
{
    public class ProfessionalExperience
    {
        public string Company { get; set; }
        public string Location { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<string> Responsibilities { get; set; }
    }




}
