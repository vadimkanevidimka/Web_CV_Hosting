namespace ProfileService.Domain.ApplicantProfile;

public class Citizenship
{
    public int Id { get; set; }
    public int ApplicantProfileId { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string UndergruondStation { get; set; }

    public ApplicantProfile ApplicantProfile { get; set; }
}