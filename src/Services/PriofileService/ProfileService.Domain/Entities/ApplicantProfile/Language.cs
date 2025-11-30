namespace ProfileService.Domain.ApplicantProfile;

public class Language
{
    public int Id { get; set; }
    public Guid ApplicantProfileId { get; set; }
    public string LanguageName { get; set; }
    public string ProficiencyLevel { get; set; } // Например, "Начинающий", "Средний", "Продвинутый"

    public ApplicantProfile ApplicantProfile { get; set; }
}