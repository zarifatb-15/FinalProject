using OnlineAuctionApp.Application.DTOs.Bids;

namespace OnlineAuctionApp.Application.Interfaces.Services;

public interface IBidService
{
    Task<BidReturnDto> PlaceBidAsync(Guid auctionId, Guid buyerId, BidCreateDto dto);

    Task<List<BidReturnDto>> GetByAuctionIdAsync(Guid auctionId);
}