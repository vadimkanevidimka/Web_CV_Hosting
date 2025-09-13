using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CVRecognizingService.Application.UseCases.Commands.Documents;
using CVRecognizingService.Application.UseCases.Queries.Documents;
using CVRecognizingService.Application.UseCases.Queries.Root;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace CVRecognizingService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DocumentsController : Controller
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;
    public DocumentsController(
        ILogger<DocumentsController> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost("upload")]
    [Authorize]
    public async Task<IActionResult> Upload(
        [FromForm]CreateDocumentCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);

        _logger.LogInformation("Document has been sucessfully uploaded");

        return !result.Contains("Failed", StringComparison.InvariantCultureIgnoreCase) ? Created(result, result) : BadRequest(result);
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAll(
        [FromQuery]GetAllDocumentsQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(query, cancellationToken);

        _logger.LogInformation("Documents were obtained");

        return result.Count() != 0 ? 
            Ok(result) 
            : NotFound();
    }

    [HttpGet("get")]
    public async Task<IActionResult> Get(
        [FromQuery]GetDocumentByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(query, cancellationToken);

        _logger.LogInformation("Extended Document was obtained");

        return result!=null ? Ok(result) : NotFound();
    }

    [HttpDelete("delete")]
    [Authorize]
    public async Task<IActionResult> Delete(
        [FromQuery]DeleteDocumentCommand command, 
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result ? Ok() : BadRequest();
    }

    [HttpGet("getfull")]
    [Authorize]
    public async Task<IActionResult> GetFull(
        [FromQuery]GetRootQuery query,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return result != null ? Ok(result) : BadRequest();
    }
}
