namespace CVHosting.Shared.Models.Entities.ApplicantProfile;

public class DriverLicense
{
    public Guid Id { get; set; }
    public int ApplicantProfileId { get; set; }
    public string Category { get; set; } // Например, "B", "C"

    public ApplicantProfile ApplicantProfile { get; set; }
}