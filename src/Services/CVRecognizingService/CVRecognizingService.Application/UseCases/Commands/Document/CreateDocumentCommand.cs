using MediatR;
using Microsoft.AspNetCore.Http;

namespace CVRecognizingService.Application.UseCases.Commands.Document;

public sealed record CreateDocumentCommand(IFormFile File)
    : IRequest<string>;
