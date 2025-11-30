namespace ProfileService.Domain.ApplicantProfile;

public class WorkExperience
{
    public int Id { get; set; }
    public Guid ApplicantProfileId { get; set; }
    public string CompanyName { get; set; }
    public string? City { get; set; }
    public string Position { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsCurrentlyWorking { get; set; }
    public string? JobDescription { get; set; }

    public ApplicantProfile ApplicantProfile { get; set; }
}