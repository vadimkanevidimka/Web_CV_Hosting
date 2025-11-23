using AutoMapper;
using ProfileService.Application.UseCases.Commands;
using ProfileService.Domain.ApplicantProfile;
using ProfileService.Infastructure.Repositories;

namespace ProfileService.Application.UseCases.CommandHandlers;

public class CreateApplicantProfileCommandHandler
{
    private readonly ProfileRepository _repository;
    private readonly IMapper _mapper;

    public CreateApplicantProfileCommandHandler(ProfileRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateApplicantProfileCommand request, CancellationToken cancellationToken)
    {
        var applicantProfile = _mapper.Map<ApplicantProfile>(request);
        await _repository.AddAsync(applicantProfile, cancellationToken);
        return applicantProfile.Id;
    }
}