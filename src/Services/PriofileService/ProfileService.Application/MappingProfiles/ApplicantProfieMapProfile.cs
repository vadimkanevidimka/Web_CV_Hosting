using AutoMapper;
using ProfileService.Application.UseCases.Commands;
using ProfileService.Domain.ApplicantProfile;

namespace ProfileService.Application.MappingProfiles;

public class ApplicantProfieMapProfile : Profile
{
    public ApplicantProfieMapProfile()
    {
        CreateMap<CreateApplicantProfileCommand, ApplicantProfile>()
            .ForMember(dest => dest.Educations, opt => opt.MapFrom(src => src.Educations))
            .ForMember(dest => dest.WorkExperiences, opt => opt.MapFrom(src => src.WorkExperiences))
            .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.Skills))
            .ForMember(dest => dest.Languages, opt => opt.MapFrom(src => src.Languages))
            .ForMember(dest => dest.Citizenships, opt => opt.MapFrom(src => src.Citizenships))
            .ForMember(dest => dest.DriverLicenses, opt => opt.MapFrom(src => src.DriverLicenses))
            .ForMember(dest => dest.PortfolioItems, opt => opt.MapFrom(src => src.PortfolioItems))
            .ForMember(dest => dest.SalaryExpectations, opt => opt.MapFrom(src => src.SalaryExpectations))
            .ForMember(dest => dest.EmploymentType, opt => opt.MapFrom(src => src.EmploymentType));
        
        CreateMap<CreateEducationCommand, Education>();
        CreateMap<CreateWorkExperienceCommand, WorkExperience>();
        CreateMap<CreateSkillCommand, Skill>();
        CreateMap<CreateLanguageCommand, Language>();
        CreateMap<CreateCitizenshipCommand, Citizenship>();
        CreateMap<CreateDriverLicenseCommand, DriverLicense>();
        CreateMap<CreatePortfolioItemCommand, PortfolioItem>();
        CreateMap<CreateSalaryExpectationsCommand, SalaryExpectations>();
        CreateMap<CreateEmploymentTypeCommand, EmploymentType>();
    }
}