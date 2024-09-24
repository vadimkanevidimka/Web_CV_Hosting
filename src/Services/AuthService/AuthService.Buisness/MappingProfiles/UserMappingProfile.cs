using AuthService.Buisness.Dtos.User;
using AuthService.DataAccess.Entities;
using AutoMapper;

namespace AuthService.Buisness.MappingProfiles;

public class UserMappingProfile : Profile
{
    public UserMappingProfile() 
    {
        CreateMap<UserLoginDto, User>()
            .ReverseMap();

        CreateMap<UserRegisterDto, User>()
            .ReverseMap();

        CreateMap<UserReadDto, User>()
            .ReverseMap();

        CreateMap<UserUpdateDto, User>()
            .ReverseMap();
    }
}