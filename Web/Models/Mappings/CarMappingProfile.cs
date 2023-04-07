using AutoMapper;
using Data.Models;

namespace Web.Models.Mappings;

public class CarMappingProfile : Profile
{
    public CarMappingProfile()
    {
        CreateMap<Car, CarViewModel>()
            .ForMember(view => view.Company, e => e.MapFrom(c => c.Model.Company.Name))
            .ForMember(view => view.Model, e => e.MapFrom(c => c.Model.Name))
            .ForMember(view => view.Year, e => e.MapFrom(c => c.Model.Year));
    }
}
