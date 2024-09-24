using AuthService.Buisness.Dtos.Tokens;
using AuthService.DataAccess.Entities;
using AutoMapper;

namespace AuthService.Buisness.MappingProfiles;

public class TokenMappingProfile : Profile
{
    public TokenMappingProfile()
    {
        CreateMap<Token, RefreshToken>()
            .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.Value))
            .ForMember(dest => dest.ExpirationTime, opt => opt.MapFrom(src => src.ExpiresIn))
            .ForMember(dest => dest.CreationTime, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}