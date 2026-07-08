namespace OnlineAuctionApp.Application.DTOs.Auctions;

public class AuctionImageReturnDto
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; } = null!;
    public bool IsPrimary { get; set; }
}