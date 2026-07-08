using OnlineAuctionApp.WebAPI.Attributes;

namespace OnlineAuctionApp.WebAPI.Requests;

public class AuctionImageUploadRequest
{
    [FileTypes("image/jpeg", "image/png", "image/webp")]
    [FileLength(5)]
    public IFormFile Image { get; set; }=null!;
}