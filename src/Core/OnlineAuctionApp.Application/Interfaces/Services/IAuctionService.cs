using OnlineAuctionApp.Application.DTOs.Auctions;

namespace OnlineAuctionApp.Application.Interfaces.Services;

public interface IAuctionService
{
    Task<List<AuctionReturnDto>> GetAllAsync(AuctionFilterDto filter);
    Task<AuctionReturnDto> GetByIdAsync(Guid id);
    Task<AuctionReturnDto> CreateAsync(AuctionCreateDto dto, Guid sellerId);
    Task<List<AuctionReturnDto>> GetBySellerIdAsync(Guid sellerId);

    Task<List<AuctionReturnDto>> GetActiveBySellerIdAsync(Guid sellerId);
    Task<List<AuctionReturnDto>> GetCompletedBySellerIdAsync(Guid sellerId);
    Task<SellerDashboardSummaryDto> GetSellerDashboardSummaryAsync(Guid sellerId);
    Task<AuctionReturnDto> UpdateAsync(Guid auctionId, AuctionUpdateDto dto, Guid sellerId);
    Task<AuctionReturnDto> CancelBySellerAsync(Guid auctionId, Guid sellerId);
}