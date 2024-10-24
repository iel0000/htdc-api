using AutoMapper;
using htdc_api.Models;
using htdc_api.Models.ViewModel;

namespace htdc_api.Mapper;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<CreateProductsViewModel, Products>()
            .ReverseMap();
    }
}