using AutoMapper;
using CVRecognizingService.Domain.DTOs.Outgoing;
using CVRecognizingService.Domain.Entities;

namespace CVRecognizingService.Application.Mappings;

public class MappingProfile 
    : Profile
{
    public MappingProfile()
    {
        CreateMap<BaseDocumentDto, BaseDocument>()
            .ReverseMap()
            .ForMember(docDto => docDto.Id, ops => ops.MapFrom(doc => doc.Id.ToString()))
            .ForMember(docDto => docDto.UserId, ops => ops.MapFrom(doc => doc.User.Id.ToString()));
    }
}
