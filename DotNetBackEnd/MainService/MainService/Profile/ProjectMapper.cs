﻿using AutoMapper;
using DoAnTotNghiep.Models.Entities;
using WebAppAPI.DTO;
using WebAppAPI.Models.Entities;
using CategoryService;
using DoAnTotNghiep.DTO;
using doantotnghiep.DTO;

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
            CreateMap<ProductDTOShow, Product>()
                .ForMember(des => des.Id, s => s.MapFrom(x => 0))
                .ForMember(des => des.CreatedDate, s => s.MapFrom(x => DateTime.UtcNow))
                .ForMember(des => des.UpdatedDate, s => s.MapFrom(x => DateTime.UtcNow))
                .ForMember(des => des.ImageDetail, s => s.MapFrom(x => String.Join(",", x.ImageDetail)));
            CreateMap<Product, ProductDTOShow>().
                AfterMap((des, source) => source.ImageDetail = des.ImageDetail?.Split(','));
            CreateMap<FeedbackDTO, Feedback>()
                .ForMember(des => des.CreatedDate, s => s.MapFrom(x => DateTime.UtcNow))
                .ForMember(des => des.UpdatedDate, s => s.MapFrom(x => DateTime.UtcNow));
        }
    }
}
