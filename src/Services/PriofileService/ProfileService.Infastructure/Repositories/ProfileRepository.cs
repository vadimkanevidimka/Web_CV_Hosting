using AutoMapper;
using ProfileService.Domain.ApplicantProfile;
using ProfileService.Infastructure.DbAccess;

namespace ProfileService.Infastructure.Repositories;

public class ProfileRepository : BaseRepository<ApplicantProfile>
{
    private readonly IMapper _mapper;
    private readonly ProfileDbContext _dbContext;

    protected ProfileRepository(ProfileDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
}