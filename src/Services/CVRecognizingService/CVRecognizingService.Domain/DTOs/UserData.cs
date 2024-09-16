using System.Text.Json.Serialization;

namespace CVRecognizingService.Domain.DTO
{
    public class UserData
    {
        public Guid Id { get; private set; }

        [JsonPropertyName("Full Name")]
        public string FullName { get; set; }

        [JsonPropertyName("Contacts")]
        public Contacts Contacts { get; set; }

        [JsonPropertyName("Location")]
        public string Location { get; set; }

        [JsonPropertyName("Links")]
        public Links Links { get; set; }

        [JsonPropertyName("Job Title")]
        public string JobTitle { get; set; }

        [JsonPropertyName("Professional Experience")]
        public List<ProfessionalExperience> ProfessionalExperience { get; set; }

        [JsonPropertyName("Education")]
        public Education Education { get; set; }

        [JsonPropertyName("Languages")]
        public Languages Languages { get; set; }

    }
}
