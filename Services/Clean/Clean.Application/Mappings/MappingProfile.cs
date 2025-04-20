using AutoMapper;
using Clean.Application.Features.Users.Commands.CreateUser;
using Clean.Domain.Entities;

namespace Clean.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateUserCommand, User>().ReverseMap();
        // CreateMap<User, UserDto>();
    }
}