using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CVRecognizingService.Application.UseCases.Commands.Documents;

public sealed record CreateDocumentCommand([FromForm]IFormFile File)
    : IRequest<string>;
