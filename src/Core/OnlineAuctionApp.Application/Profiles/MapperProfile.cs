using AutoMapper;
using OnlineAuctionApp.Application.DTOs.Categories;
using OnlineAuctionApp.Domain.Entities;

namespace OnlineAuctionApp.Application.Profiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Category, CategoryReturnDto>();

        CreateMap<CategoryCreateDto, Category>();

        CreateMap<CategoryUpdateDto, Category>();
    }
}