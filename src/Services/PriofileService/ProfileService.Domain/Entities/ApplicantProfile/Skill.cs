namespace ProfileService.Domain.ApplicantProfile;

public class Skill
{
    public int Id { get; set; }
    public Guid ApplicantProfileId { get; set; }
    public string SkillName { get; set; }

    public ApplicantProfile ApplicantProfile { get; set; }
}