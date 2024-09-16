using System.Text.Json.Serialization;

namespace CVRecognizingService.Domain.DTO
{
    public class Education
    {
        [JsonPropertyName("Degree")]
        public string Degree { get; set; }

        [JsonPropertyName("University")]
        public string University { get; set; }

        [JsonPropertyName("Year")]
        public int? Year { get; set; }
    }
}
