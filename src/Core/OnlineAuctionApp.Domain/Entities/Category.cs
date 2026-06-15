namespace OnlineAuctionApp.Domain.Entities;
using OnlineAuctionApp.Domain.Common;
using OnlineAuctionApp.Domain.Enums;
public class Category:BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public List<Auction> Auctions { get; set; } = new List<Auction>();
}