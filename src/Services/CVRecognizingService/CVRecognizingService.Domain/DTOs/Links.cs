using System.Text.Json.Serialization;

namespace CVRecognizingService.Domain.DTO
{
    public class Links
    {
        [JsonPropertyName("GitHub")]
        public string GitHub { get; set; }
    }
}
