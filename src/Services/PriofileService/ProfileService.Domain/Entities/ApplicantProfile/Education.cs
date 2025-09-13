namespace ProfileService.Domain.ApplicantProfile;

public class Education
{
    public int Id { get; set; }
    public int ApplicantProfileId { get; set; }
    public string InstitutionName { get; set; }
    public string? Specialization { get; set; }
    public string Degree { get; set; } // Например, "Бакалавр", "Магистр"
    public int? StartYear { get; set; }
    public int? EndYear { get; set; }

    public ApplicantProfile ApplicantProfile { get; set; }
}