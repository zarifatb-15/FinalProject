using OnlineAuctionApp.Application.DTOs.Auctions;

namespace OnlineAuctionApp.Application.Interfaces.Services;

public interface IAuctionClosingService
{
    Task<int> CloseExpiredAuctionsAsync();

    Task<AuctionReturnDto> CloseAuctionAsync(Guid auctionId);
}