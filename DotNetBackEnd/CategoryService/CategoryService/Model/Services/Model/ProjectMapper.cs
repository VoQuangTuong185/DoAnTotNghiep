using CategoryService;
using AutoMapper;
using CategoryService.Data.Entities;
using CategoryService.DTO;

namespace CategoryService.Model.Services.Model
{
    public class ProjectMapper : Profile
    {
        public ProjectMapper()
        {
            CreateMap<Category, CategoryReadDto>();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryReadDto, CategoryPublishDto>();
            CreateMap<Category, CategoryPublishDto>();
            CreateMap<Category, CategoryUpdateDto>();
            CreateMap<Category, GrpcCategoryModel>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image));
        }
    }
}
