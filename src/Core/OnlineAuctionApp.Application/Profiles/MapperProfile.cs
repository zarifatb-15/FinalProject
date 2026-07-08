using AutoMapper;
using OnlineAuctionApp.Application.DTOs.Categories;
using OnlineAuctionApp.Application.DTOs.Auctions;
using OnlineAuctionApp.Domain.Entities;

namespace OnlineAuctionApp.Application.Profiles;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Category, CategoryReturnDto>();

        CreateMap<CategoryCreateDto, Category>();

        CreateMap<CategoryUpdateDto, Category>();

        CreateMap<Auction, AuctionReturnDto>()
        .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
        .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

        CreateMap<AuctionCreateDto, Auction>();
        
        CreateMap<AuctionImage, AuctionImageReturnDto>();
    }
}