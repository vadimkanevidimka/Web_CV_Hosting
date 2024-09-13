using CVRecognizingService.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace CVRecognizingService.Application.Services.Interfaces
{
    public interface IDocumentService
    {
        public Task<BaseDocument> AddDocument(IFormFile file, CancellationToken cancellationToken);
        public Task<IEnumerable<BaseDocument>> GetDocuments(CancellationToken cancellationToken);
    }
}
