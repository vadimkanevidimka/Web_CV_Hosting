using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Application.UseCases.Commands;
using ProfileService.Application.UseCases.Queries;

namespace ProfileService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfilesController
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public ProfilesController(
        ILogger<ProfilesController> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IResult> Create(CreateApplicantProfileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(request, cancellationToken);
            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Results.BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route("all")]
    public async Task<IResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetAllApplicantProfilesQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Results.BadRequest(ex.Message);
        }
    }
}