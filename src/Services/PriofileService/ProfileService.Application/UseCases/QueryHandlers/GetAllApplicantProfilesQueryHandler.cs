using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProfileService.Application.DTOs;
using ProfileService.Infastructure.DbAccess;

namespace ProfileService.Application.UseCases.QueryHandlers;

public class GetAllApplicantProfilesQueryHandler 
    : IRequestHandler<Queries.GetAllApplicantProfilesQuery, IEnumerable<ApplicantProfileCardDto>>
{
    private readonly ILogger<GetAllApplicantProfilesQueryHandler> _logger;
    private readonly ProfileDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAllApplicantProfilesQueryHandler(
        ILogger<GetAllApplicantProfilesQueryHandler> logger,
        ProfileDbContext dbContext,
        IMapper mapper)
    {
        _logger = logger;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ApplicantProfileCardDto>> Handle(
        Queries.GetAllApplicantProfilesQuery request, 
        CancellationToken cancellationToken)
    {
        var profiles = await _dbContext.ApplicantProfiles
            .Include(p => p.Educations)
            .Include(p => p.WorkExperiences)
            .Include(p => p.Skills)
            .Include(p => p.Languages)
            .Include(p => p.SalaryExpectations)
            .Include(p => p.EmploymentType)
            .AsNoTracking()
            .ProjectTo<ApplicantProfileCardDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
            
        _logger.LogInformation($"Retrieved {profiles.Count} applicant profile cards");
        return profiles;
    }
}
