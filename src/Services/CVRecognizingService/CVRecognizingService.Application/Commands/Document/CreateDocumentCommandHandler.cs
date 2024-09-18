using CSharpFunctionalExtensions;
using CVRecognizingService.Application.Services.Implementation;
using CVRecognizingService.Application.Services.Interfaces;
using Events_Web_application.Application.Services.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CVRecognizingService.Application.Commands.Document.Create;

public class CreateDocumentCommandHandler 
    : IRequestHandler<CreateDocumentCommand, string>
{
    private readonly ILogger<CreateDocumentCommandHandler> _logger;
    private readonly DocumentService _documentService;

    public CreateDocumentCommandHandler(
        ILogger<CreateDocumentCommandHandler> logger,
        IService documentService)
    {
        _logger = logger;
        _documentService = (DocumentService)documentService;
    }

    public async Task<string> Handle(
        CreateDocumentCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"{request.File.FileName} started recognizing");
            var result = await _documentService.AddDocument(request.File, cancellationToken);
            return result.Id.ToString();
        }
        catch (ServiceException ex)
        {
            _logger.LogInformation($"{request.File.FileName} finished recognizing with error: {ex.Value}");
            return $"Failed service operation: {ex.Operation}, error: {ex.Message} with value: {ex.Value}";
        }
    }
}
