using System.Text.Json.Serialization;

namespace CVRecognizingService.Domain.DTO
{

    public class Languages
    {
        [JsonPropertyName("Programming Languages")]
        public List<string> ProgrammingLanguages { get; set; }

        [JsonPropertyName("Back-end")]
        public List<string> Backend { get; set; }

        [JsonPropertyName("Front-end")]
        public List<string> Frontend { get; set; }

        [JsonPropertyName("Infrastructure")]
        public List<string> Infrastructure { get; set; }

        [JsonPropertyName("Metrics")]
        public List<string> Metrics { get; set; }
    }
}
