using System.Text.Json.Serialization;

namespace CVRecognizingService.Domain.DTO
{
    public class Contacts
    {
        [JsonPropertyName("Phone")]
        public string Phone { get; set; }

        [JsonPropertyName("Email")]
        public string Email { get; set; }

        [JsonPropertyName("LinkedIn")]
        public string LinkedIn { get; set; }
    }
}
