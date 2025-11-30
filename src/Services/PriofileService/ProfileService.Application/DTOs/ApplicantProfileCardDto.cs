namespace ProfileService.Application.DTOs;

public class ApplicantProfileCardDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public string? PhotoUrl { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset LastModifiedAt { get; set; }
    
    // Summary information
    public int EducationCount { get; set; }
    public int WorkExperienceCount { get; set; }
    public int SkillsCount { get; set; }
    public int LanguagesCount { get; set; }
    
    // Latest work experience summary
    public string? CurrentPosition { get; set; }
    public string? CurrentCompany { get; set; }
    
    // Salary expectations
    public decimal? SalaryAmount { get; set; }
    public string? SalaryCurrency { get; set; }
    
    // Employment type
    public string? EmploymentType { get; set; }
}
