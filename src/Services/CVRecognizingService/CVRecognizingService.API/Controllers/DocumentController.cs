using CVRecognizingService.Application.Helpers.AICompanion;
using CVRecognizingService.Application.Services.Implementation;
using CVRecognizingService.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CVRecognizingService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : Controller
    {
        private readonly ILogger _logger;
        private readonly DocumentService _documentService;

        private readonly GeminiAITextChat _chat;
        public DocumentController(ILogger<DocumentController> logger, IDocumentService documentService) 
        {
            _logger = logger;
            _documentService = (DocumentService)documentService;
        }

        [HttpPost("upload")]
        public async Task<IResult> Upload(IFormFile file, CancellationToken cancellationToken = default)
        {
            try
            {
                return Results.Ok(await _documentService.AddDocument(file, cancellationToken));
            }
            catch(Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        [HttpPost("GetAll")]
        public async Task<IResult> GetAll(CancellationToken cancellationToken = default)
        {
            try
            {
                return Results.Ok(await _documentService.GetDocuments(cancellationToken));
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
            
    }
}
