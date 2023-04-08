using AutoMapper;
using Data.Models;

namespace Web.Models.Mappings;

public class CarMappingProfile : Profile
{
    public CarMappingProfile()
    {
        CreateMap<Car, CarViewModel>()
            .ForMember(v => v.Company, e => e.MapFrom(c => c.Model.Company.Name))
            .ForMember(v => v.Model, e => e.MapFrom(c => c.Model.Name))
            .ForMember(v => v.Year, e => e.MapFrom(c => c.Model.Year));

        CreateMap<CarCreateModel, Car>()
            .ForMember(c => c.Picture, e => e.Ignore())
            .ForMember(c => c.PictureType, e => e.MapFrom(v => v.Picture.ContentType))
            .ForPath(c => c.Model.Name, e => e.MapFrom(v => v.Model))
            .ForPath(c => c.Model.Year, e => e.MapFrom(v => v.Year))
            .ForPath(c => c.Model.Company.Name, e => e.MapFrom(v => v.Company));

        CreateMap<Car, CarEditModel>()
            .ForMember(v => v.Company, e => e.MapFrom(c => c.Model.Company.Name))
            .ForMember(v => v.Model, e => e.MapFrom(c => c.Model.Name))
            .ForMember(v => v.Year, e => e.MapFrom(c => c.Model.Year))
            .ForMember(v => v.Picture, e => e.Ignore())
            .ReverseMap()
            .ForMember(c => c.Picture, e => e.Ignore())
            .ForPath(c => c.Model.Name, e => e.MapFrom(v => v.Model));
    }
}
