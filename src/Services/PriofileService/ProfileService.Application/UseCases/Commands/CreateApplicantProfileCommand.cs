using MediatR;

namespace ProfileService.Application.UseCases.Commands;

public class CreateApplicantProfileCommand : IRequest<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PhotoUrl { get; set; }
    public string? AboutMe { get; set; }
    public ICollection<CreateEducationCommand> Educations { get; set; } = new List<CreateEducationCommand>();
    public ICollection<CreateWorkExperienceCommand> WorkExperiences { get; set; } = new List<CreateWorkExperienceCommand>();
    public ICollection<CreateSkillCommand> Skills { get; set; } = new List<CreateSkillCommand>();
    public ICollection<CreateLanguageCommand> Languages { get; set; } = new List<CreateLanguageCommand>();
    public ICollection<CreateCitizenshipCommand> Citizenships { get; set; } = new List<CreateCitizenshipCommand>();
    public ICollection<CreateDriverLicenseCommand> DriverLicenses { get; set; } = new List<CreateDriverLicenseCommand>();
    public ICollection<CreatePortfolioItemCommand> PortfolioItems { get; set; } = new List<CreatePortfolioItemCommand>();

    // Ожидания по зарплате
    public CreateSalaryExpectationsCommand? SalaryExpectations { get; set; }

    // Тип занятости
    public CreateEmploymentTypeCommand? EmploymentType { get; set; }
}