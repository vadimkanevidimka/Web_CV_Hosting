namespace CVHosting.Shared.Models.Entities.ApplicantProfile;

public class EmploymentType
{
    public Guid Id { get; set; }
    public int ApplicantProfileId { get; set; }
    public string Type { get; set; } // "Full-time", "Part-time", "Project", "Internship"

    public ApplicantProfile ApplicantProfile { get; set; }
}