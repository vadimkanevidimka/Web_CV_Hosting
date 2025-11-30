using AutoMapper;
using ProfileService.Application.UseCases.Commands;
using ProfileService.Application.DTOs;
using ProfileService.Domain.ApplicantProfile;

namespace ProfileService.Application.MappingProfiles;

public class ApplicantProfieMapProfile : Profile
{
    public ApplicantProfieMapProfile()
    {
        // Self-mapping for query projections
        CreateMap<ApplicantProfile, ApplicantProfile>();
        
        // Mapping to card DTO for list views
        CreateMap<ApplicantProfile, ApplicantProfileCardDto>()
            .ForMember(dest => dest.EducationCount, opt => opt.MapFrom(src => src.Educations != null ? src.Educations.Count : 0))
            .ForMember(dest => dest.WorkExperienceCount, opt => opt.MapFrom(src => src.WorkExperiences != null ? src.WorkExperiences.Count : 0))
            .ForMember(dest => dest.SkillsCount, opt => opt.MapFrom(src => src.Skills != null ? src.Skills.Count : 0))
            .ForMember(dest => dest.LanguagesCount, opt => opt.MapFrom(src => src.Languages != null ? src.Languages.Count : 0))
            .ForMember(dest => dest.CurrentPosition, opt => opt.MapFrom(src => 
                src.WorkExperiences != null && src.WorkExperiences.Any() 
                    ? src.WorkExperiences.OrderByDescending(w => w.StartDate).FirstOrDefault()!.Position 
                    : null))
            .ForMember(dest => dest.CurrentCompany, opt => opt.MapFrom(src => 
                src.WorkExperiences != null && src.WorkExperiences.Any() 
                    ? src.WorkExperiences.OrderByDescending(w => w.StartDate).FirstOrDefault()!.CompanyName 
                    : null))
            .ForMember(dest => dest.SalaryAmount, opt => opt.MapFrom(src => src.SalaryExpectations != null ? src.SalaryExpectations.Amount : null))
            .ForMember(dest => dest.SalaryCurrency, opt => opt.MapFrom(src => src.SalaryExpectations != null ? src.SalaryExpectations.Currency : null))
            .ForMember(dest => dest.EmploymentType, opt => opt.MapFrom(src => src.EmploymentType != null ? src.EmploymentType.Type : null));
        
        CreateMap<CreateApplicantProfileCommand, ApplicantProfile>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.LastModifiedAt, opt => opt.MapFrom(src => DateTimeOffset.UtcNow))
            .ForMember(dest => dest.Educations, opt => opt.MapFrom(src => src.Educations))
            .ForMember(dest => dest.WorkExperiences, opt => opt.MapFrom(src => src.WorkExperiences))
            .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.Skills))
            .ForMember(dest => dest.Languages, opt => opt.MapFrom(src => src.Languages))
            .ForMember(dest => dest.Citizenships, opt => opt.MapFrom(src => src.Citizenships))
            .ForMember(dest => dest.DriverLicenses, opt => opt.MapFrom(src => src.DriverLicenses))
            .ForMember(dest => dest.PortfolioItems, opt => opt.MapFrom(src => src.PortfolioItems))
            .ForMember(dest => dest.SalaryExpectations, opt => opt.MapFrom(src => src.SalaryExpectations))
            .ForMember(dest => dest.EmploymentType, opt => opt.MapFrom(src => src.EmploymentType))
            .AfterMap((src, dest) =>
            {
                // Set ApplicantProfileId for all child entities (navigation properties are not needed for EF Core)
                if (dest.Educations != null)
                {
                    foreach (var education in dest.Educations)
                    {
                        education.ApplicantProfileId = dest.Id;
                    }
                }
                
                if (dest.WorkExperiences != null)
                {
                    foreach (var workExperience in dest.WorkExperiences)
                    {
                        workExperience.ApplicantProfileId = dest.Id;
                    }
                }
                
                if (dest.Skills != null)
                {
                    foreach (var skill in dest.Skills)
                    {
                        skill.ApplicantProfileId = dest.Id;
                    }
                }
                
                if (dest.Languages != null)
                {
                    foreach (var language in dest.Languages)
                    {
                        language.ApplicantProfileId = dest.Id;
                    }
                }
                
                if (dest.Citizenships != null)
                {
                    foreach (var citizenship in dest.Citizenships)
                    {
                        citizenship.ApplicantProfileId = dest.Id;
                    }
                }
                
                if (dest.DriverLicenses != null)
                {
                    foreach (var driverLicense in dest.DriverLicenses)
                    {
                        driverLicense.ApplicantProfileId = dest.Id;
                    }
                }
                
                if (dest.PortfolioItems != null)
                {
                    foreach (var portfolioItem in dest.PortfolioItems)
                    {
                        portfolioItem.ApplicantProfileId = dest.Id;
                    }
                }
                
                if (dest.SalaryExpectations != null)
                {
                    dest.SalaryExpectations.ApplicantProfileId = dest.Id;
                }
                
                if (dest.EmploymentType != null)
                {
                    dest.EmploymentType.ApplicantProfileId = dest.Id;
                }
            });

        CreateMap<CreateEducationCommand, Education>();
        CreateMap<CreateWorkExperienceCommand, WorkExperience>()
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.StartDate, DateTimeKind.Utc)))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.HasValue ? DateTime.SpecifyKind(src.EndDate.Value, DateTimeKind.Utc) : (DateTime?)null));
        CreateMap<CreateSkillCommand, Skill>();
        CreateMap<CreateLanguageCommand, Language>();
        CreateMap<CreateCitizenshipCommand, Citizenship>();
        CreateMap<CreateDriverLicenseCommand, DriverLicense>();
        CreateMap<CreatePortfolioItemCommand, PortfolioItem>();
        CreateMap<CreateSalaryExpectationsCommand, SalaryExpectations>();
        CreateMap<CreateEmploymentTypeCommand, EmploymentType>();
    }
}