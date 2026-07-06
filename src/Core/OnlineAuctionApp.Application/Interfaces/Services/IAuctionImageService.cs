using OnlineAuctionApp.Application.DTOs.Auctions;

namespace OnlineAuctionApp.Application.Interfaces.Services;

public interface IAuctionImageService
{
    Task<AuctionImageReturnDto> AddImageAsync(Guid auctionId, Stream fileStream, string fileName, Guid sellerId);
}