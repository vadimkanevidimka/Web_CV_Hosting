namespace CVRecognizingService.Domain.DTOs.Incoming
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class AboutMe
    {
        public string Summary { get; set; }
        public List<string> PersonalQualities { get; set; }
        public string CareerGoals { get; set; }
    }




}
