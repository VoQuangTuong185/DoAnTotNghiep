using AutoMapper;
using Microsoft.CodeAnalysis;
using THUCTAPTOTNGHIEP.DTOM;
using THUCTAPTOTNGHIEP.Models.Entities;
using WebAppAPI.DTO;
using WebAppAPI.Models.Entities;
using CategoryService;
using THUCTAPTOTNGHIEP.DTO;

namespace WebAppAPI.Services.Model
{
    public class ProjectMapper : Profile
    {
        public ProjectMapper() 
        {
            CreateMap<RegisterUserDTO, User>();
            CreateMap<User, RegisterUserDTO>();
            CreateMap<UserPermisson, UserAPI>();
            CreateMap<UserAPI, UserPermisson>();
            CreateMap<ProductDTO, Product>()
                .ForMember(des => des.Id, s => s.MapFrom(x => 0))
                .ForMember(des => des.CreatedDate, s => s.MapFrom(x => DateTime.UtcNow))
                .ForMember(des => des.UpdatedDate, s => s.MapFrom(x => DateTime.UtcNow));
            CreateMap<Product, ProductDTO>();
            CreateMap<CategoryDTO, Category>()
                .ForMember(des => des.Id, s => s.MapFrom(x => 0));
            CreateMap<Category, CategoryDTO>();

            CreateMap<Category, CategoryReadDto>();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<Product, ProductReadDto>();
            CreateMap<CategoryPublishedDto, Category>()
                .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<GrpcCategoryModel, Category>()
                .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(dest => dest.Products, opt => opt.Ignore());
            CreateMap<CategoryUpdateDto, Category>()
                .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.Id));
            CreateMap<BrandDTO, Brand>();
            CreateMap<CreateBrandDTO, Brand>();
        }
    }
}
