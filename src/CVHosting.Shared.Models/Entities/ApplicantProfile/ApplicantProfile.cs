using CVHosting.Shared.Models.Entities;

namespace CVHosting.Shared.Models.Entities.ApplicantProfile;

public class ApplicantProfile : IBaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset LastModifiedAt { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PhotoUrl { get; set; }

    public ICollection<Education> Educations { get; set; }
    public ICollection<WorkExperience> WorkExperiences { get; set; }
    public ICollection<Skill> Skills { get; set; }
    public ICollection<Language> Languages { get; set; }
    public ICollection<Citizenship> Citizenships { get; set; }
    public ICollection<DriverLicense> DriverLicenses { get; set; }
    public ICollection<PortfolioItem> PortfolioItems { get; set; }
    public SalaryExpectations? SalaryExpectations { get; set; }
    public EmploymentType? EmploymentType { get; set; }
}