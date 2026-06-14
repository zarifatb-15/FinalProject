namespace OnlineAuctionApp.Domain.Entities;
public class Auction:BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal StartingPrice { get; set; }
    public decimal CurrentPrice { get; set; }
    public DateTime EndTime { get; set; }

    public bool IsActive { get; set; } = true;

    public Guid SellerId { get; set; }
    public AppUser Seller { get; set; } = null!;

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public List<Bid> Bids { get; set; } = new List<Bid>();
}