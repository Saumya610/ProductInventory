using AutoMapper;
using WebApp_Assessment.Source.DTOs;
using WebApp_Assessment.Source.Models;

namespace WebApp_Assessment.Source
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<ProductUpdateDto, Product>();
        }
    }
}
