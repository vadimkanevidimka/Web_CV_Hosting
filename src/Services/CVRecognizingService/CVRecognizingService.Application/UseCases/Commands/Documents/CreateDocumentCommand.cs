using CVRecognizingService.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CVRecognizingService.Application.UseCases.Commands.Documents;

public sealed record CreateDocumentCommand([FromForm] IFormFile File, string UserId)
    : IRequest<CreateDocumentResponse>
{ }

public sealed record CreateDocumentCommandDto([FromForm] IFormFile File) { };
