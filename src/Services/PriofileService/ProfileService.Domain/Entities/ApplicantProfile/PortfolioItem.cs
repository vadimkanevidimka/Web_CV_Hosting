namespace ProfileService.Domain.ApplicantProfile;

public class PortfolioItem
{
    public int Id { get; set; }
    public Guid ApplicantProfileId { get; set; }
    public string ProjectName { get; set; }
    public string Url { get; set; }
    public string? Description { get; set; }

    public ApplicantProfile ApplicantProfile { get; set; }
}