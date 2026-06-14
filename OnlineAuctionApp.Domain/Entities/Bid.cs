namespace OnlineAuctionApp.Domain.Entities;
public class Bid:BaseEntity
{
    public decimal Amount { get; set; }
    public DateTime BidTime { get; set; } = DateTime.UtcNow;

    public Guid AuctionId { get; set; }
    public Auction Auction { get; set; } = null!;

    public Guid BuyerId { get; set; }
    public AppUser Buyer { get; set; } = null!;
}

