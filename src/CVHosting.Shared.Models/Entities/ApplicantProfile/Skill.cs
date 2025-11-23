namespace CVHosting.Shared.Models.Entities.ApplicantProfile;

public class Skill
{
    public Guid Id { get; set; }
    public int ApplicantProfileId { get; set; }
    public string SkillName { get; set; }

    public ApplicantProfile ApplicantProfile { get; set; }
}