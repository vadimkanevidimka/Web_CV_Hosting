namespace ProfileService.Domain.ApplicantProfile;

public class EmploymentType
{
    public int Id { get; set; }
    public int ApplicantProfileId { get; set; }
    public string Type { get; set; } // "Full-time", "Part-time", "Project", "Internship"

    public ApplicantProfile ApplicantProfile { get; set; }
}