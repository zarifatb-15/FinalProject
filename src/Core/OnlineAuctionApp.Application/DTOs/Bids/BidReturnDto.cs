namespace OnlineAuctionApp.Application.DTOs.Bids;

public class BidReturnDto
{
    public Guid Id { get; set; }

    public Guid AuctionId { get; set; }

    public Guid BuyerId { get; set; }

    public string BuyerUsername { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime BidTime { get; set; }
}