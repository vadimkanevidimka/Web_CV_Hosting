namespace CVHosting.Shared.Models.Entities.ApplicantProfile;

public class WorkExperience
{
    public Guid Id { get; set; }
    public int ApplicantProfileId { get; set; }
    public string CompanyName { get; set; }
    public string? City { get; set; }
    public string Position { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrentlyWorking { get; set; }
    public string? JobDescription { get; set; }

    public ApplicantProfile ApplicantProfile { get; set; }
}