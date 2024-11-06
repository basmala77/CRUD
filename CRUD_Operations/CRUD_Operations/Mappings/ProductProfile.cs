using AutoMapper;
using CRUD_Operations.Data;
using CRUD_Operations.Dto;

namespace CRUD_Operations.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>();
        }
    }
}
