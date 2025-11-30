namespace ProfileService.Domain.ApplicantProfile;

public class DriverLicense
{
    public int Id { get; set; }
    public Guid ApplicantProfileId { get; set; }
    public string Category { get; set; } // Например, "B", "C"

    public ApplicantProfile ApplicantProfile { get; set; }
}