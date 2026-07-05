using OnlineAuctionApp.Application.DTOs.Auctions;

namespace OnlineAuctionApp.Application.Interfaces.Services;

public interface IAuctionService
{
    Task<List<AuctionReturnDto>> GetAllAsync();
    Task<AuctionReturnDto> GetByIdAsync(Guid id);
    Task<AuctionReturnDto> CreateAsync(AuctionCreateDto dto, Guid sellerId);
    Task<List<AuctionReturnDto>> GetBySellerIdAsync(Guid sellerId);
}