namespace OnlineAuctionApp.Domain.Entities;
using OnlineAuctionApp.Domain.Common;
using OnlineAuctionApp.Domain.Enums;
public class AuctionImage : BaseEntity
{
    public string ImageUrl { get; set; } = null!;
    public bool IsPrimary { get; set; }
    public Guid AuctionId { get; set; }
    public Auction Auction { get; set; } = null!;
}