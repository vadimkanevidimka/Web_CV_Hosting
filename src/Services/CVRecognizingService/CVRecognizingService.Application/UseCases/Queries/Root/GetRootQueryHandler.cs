using CVRecognizingService.Domain.Abstracts.Repo;
using CVRecognizingService.Domain.Entities;
using CVRecognizingService.Infrastructure.DataAccess.Repositories;
using Events_Web_application.Application.Services.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System.Text.Json;

namespace CVRecognizingService.Application.UseCases.Queries.Root;

internal class GetRootQueryHandler
    : IRequestHandler<GetRootQuery, Domain.DTOs.Incoming.Root>
{
    private readonly ILogger<GetRootQueryHandler> _logger;
    private readonly ProcessedDataRepository _processedDataRepository;

    public GetRootQueryHandler(
        ILogger<GetRootQueryHandler> logger,
        IRepository<ProcessedData> processedDataRepository)
    {
        _logger = logger;
        _processedDataRepository = (ProcessedDataRepository)processedDataRepository;
    }
    public async Task<Domain.DTOs.Incoming.Root> Handle(GetRootQuery query, CancellationToken cancellationToken)
    {
        return await GetDataFromDocument(ObjectId.Parse(query.DocId), cancellationToken);
    }

    public async Task<Domain.DTOs.Incoming.Root> GetDataFromDocument(ObjectId id, CancellationToken cancellationToken)
    {
        if (id == ObjectId.Empty) throw new ServiceException(nameof(Handle), id, "Id is not correct or not found");
        var result = await _processedDataRepository.GetDataByDocId(id, cancellationToken);

        var rootelement = JsonSerializer.Deserialize<Domain.DTOs.Incoming.Root>(result.StructuredData);
        return rootelement;
    }
}
