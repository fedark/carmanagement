using AutoMapper;
using Data.Models;

namespace WebApi.Models.Mappings;

public class CarMappingProfile : Profile
{
    public CarMappingProfile()
    {
        CreateMap<CarRequestModel, Car>()
            .ForMember(c => c.Picture, e => e.MapFrom(c => new byte[] { 1 }))
            .ForMember(c => c.PictureType, e => e.MapFrom(c => "image/png"))
            .ForPath(c => c.Model.Name, e => e.MapFrom(c => c.Model))
            .ForPath(c => c.Model.Year, e => e.MapFrom(c => c.Year))
            .ForPath(c => c.Model.Company.Name, e => e.MapFrom(c => c.Company));
    }
}
