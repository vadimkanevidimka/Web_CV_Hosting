using CVRecognizingService.Application.Commands.Document;
using CVRecognizingService.Application.Commands.Document.Create;
using CVRecognizingService.Application.Queries.Documents;
using CVRecognizingService.Application.Queries.Root;
using CVRecognizingService.Application.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CVRecognizingService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : Controller
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        public DocumentsController(
            ILogger<DocumentsController> logger,
            IService documentService, IMediator mediator)
        {
            _logger = logger;
            _mediator = (Mediator)mediator;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(
            [FromForm]CreateDocumentCommand command, 
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);

            _logger.LogInformation("Document has been sucessfully uploaded");

            return result.IsSuccess ? Created() : BadRequest();
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll(
            [FromQuery]GetAllDocumentsQuery query,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(query, cancellationToken);

            _logger.LogInformation("Documents were obtained");

            return result.Count() != 0 ? Ok(result) : NotFound();
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
        public async Task<IActionResult> Delete(
            [FromQuery]DeleteDocumentCommand command, 
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result ? Ok() : BadRequest();
        }

        [HttpGet("getfull")]
        public async Task<IActionResult> GetFull(
            [FromQuery]GetRootQuery query,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return result != null ? Ok(result) : BadRequest();
        }
    }
}
