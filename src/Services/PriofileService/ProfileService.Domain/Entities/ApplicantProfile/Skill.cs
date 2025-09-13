namespace ProfileService.Domain.ApplicantProfile;

public class Skill
{
    public int Id { get; set; }
    public int ApplicantProfileId { get; set; }
    public string SkillName { get; set; }

    public ApplicantProfile ApplicantProfile { get; set; }
}