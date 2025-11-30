using MediatR;
using ProfileService.Application.DTOs;

namespace ProfileService.Application.UseCases.Queries;

public class GetAllApplicantProfilesQuery : IRequest<IEnumerable<ApplicantProfileCardDto>>
{
}
