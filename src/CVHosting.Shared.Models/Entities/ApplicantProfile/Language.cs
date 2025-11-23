namespace CVHosting.Shared.Models.Entities.ApplicantProfile;

public class Language
{
    public Guid Id { get; set; }
    public int ApplicantProfileId { get; set; }
    public string LanguageName { get; set; }
    public string ProficiencyLevel { get; set; } // Например, "Начинающий", "Средний", "Продвинутый"

    public ApplicantProfile ApplicantProfile { get; set; }
}