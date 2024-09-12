using System.Text.Json.Serialization;

namespace CVRecognizingService.Domain.DTO
{
    public class ProfessionalExperience
    {
        [JsonPropertyName("Company")]
        public string Company { get; set; }

        [JsonPropertyName("Location")]
        public string Location { get; set; }

        [JsonPropertyName("Start Date")]
        public string StartDate { get; set; }

        [JsonPropertyName("End Date")]
        public string EndDate { get; set; }

        [JsonPropertyName("Responsibilities")]
        public List<string> Responsibilities { get; set; }
    }
}
