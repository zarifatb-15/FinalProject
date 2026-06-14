namespace OnlineAuctionApp.Domain.Entities;
public class Category:BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public List<Auction> Auctions { get; set; } = new List<Auction>();
}