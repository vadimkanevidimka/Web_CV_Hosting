using CSharpFunctionalExtensions;
using CVRecognizingService.Application.Commands.Document.Create;
using CVRecognizingService.Application.Services.Implementation;
using CVRecognizingService.Application.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace CVRecognizingService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : Controller
    {
        private readonly ILogger _logger;
        private readonly DocumentService _documentService;
        private readonly IMediator _mediator;
        public DocumentController(
            ILogger<DocumentController> logger,
            IService documentService, IMediator mediator)
        {
            _logger = logger;
            _documentService = (DocumentService)documentService;
            _mediator = (Mediator)mediator;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm]CreateDocumentCommand command, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.IsSuccess ? Ok() : BadRequest();
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            try
            {
                return Ok(await _documentService.GetDocuments(cancellationToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get/{id:length(24)}")]
        public async Task<IActionResult> Get(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                return Ok(await _documentService.GetById(ObjectId.Parse(id) ,cancellationToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete{id:length(24)}")]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken = default)
        {
            try
            {
                return Ok(await _documentService.Delete(ObjectId.Parse(id),cancellationToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
