namespace CVHosting.Shared.Models.Entities.ApplicantProfile;

public class PortfolioItem
{
    public Guid Id { get; set; }
    public int ApplicantProfileId { get; set; }
    public string ProjectName { get; set; }
    public string Url { get; set; }
    public string? Description { get; set; }

    public ApplicantProfile ApplicantProfile { get; set; }
}