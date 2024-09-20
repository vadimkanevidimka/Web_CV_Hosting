using CSharpFunctionalExtensions;
using CVRecognizingService.Application.Services.Implementation;
using CVRecognizingService.Application.Services.Interfaces;
using Events_Web_application.Application.Services.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CVRecognizingService.Application.Commands.Document.Create;

public class CreateDocumentCommandHandler 
    : IRequestHandler<CreateDocumentCommand, Result>
{
    private readonly ILogger<CreateDocumentCommandHandler> _logger;
    private readonly DocumentService _documentService;

    public CreateDocumentCommandHandler(ILogger<CreateDocumentCommandHandler> logger, IService documentService)
    {
        _logger = logger;
        _documentService = (DocumentService)documentService;
    }

    public async Task<Result> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation($"{request.File.FileName} started recognizing");
            return Result.Success(await _documentService.AddDocument(request.File, cancellationToken));
        }
        catch (ServiceException ex)
        {
            _logger.LogInformation($"{request.File.FileName} finished recognizing with error: {ex.Value}");
            return Result.Failure($"Failed service operation: {ex.Operation}, error: {ex.Message} with value: {ex.Value}");
        }
    }
}
