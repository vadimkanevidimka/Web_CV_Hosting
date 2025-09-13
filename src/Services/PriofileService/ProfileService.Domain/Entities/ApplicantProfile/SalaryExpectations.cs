namespace ProfileService.Domain.ApplicantProfile;

public class SalaryExpectations
{
    public int Id { get; set; }
    public int ApplicantProfileId { get; set; }
    public decimal? Amount { get; set; }
    public string Currency { get; set; }
    public string? Type { get; set; } // Например, "Gross" или "Net"

    public ApplicantProfile ApplicantProfile { get; set; }
}