using AutoMapper;
using Data.Models;

namespace WebApi.Models.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserResponceModel>();
    }
}
