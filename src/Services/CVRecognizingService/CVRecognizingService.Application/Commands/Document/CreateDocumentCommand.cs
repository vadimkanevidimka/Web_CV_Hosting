using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CVRecognizingService.Application.Commands.Document.Create;

public sealed record CreateDocumentCommand(IFormFile File) 
    : IRequest<Result>;
