using AutoMapper;
using Core.Models.NovaPoshta.City;
using Core.Models.NovaPoshta.Department;
using Domain.Entities.Delivery;

namespace Core.Mapper;

public class NovaPoshtaMapper : Profile
{
    public NovaPoshtaMapper()
    {
        CreateMap<CityItemResponse, CityEntity>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Description));

        CreateMap<DepartmentItemResponse, PostDepartmentEntity>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Description));
    }

}