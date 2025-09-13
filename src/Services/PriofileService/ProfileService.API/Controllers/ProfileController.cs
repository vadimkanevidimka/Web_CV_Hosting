using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Application.UseCases.Commands;

namespace ProfileService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfileController
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;
    
    public ProfileController(
        ILogger<ProfileController> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IResult> Create(CreateApplicantProfileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _mediator.Send(request, cancellationToken);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return Results.BadRequest();
        }
    }
}