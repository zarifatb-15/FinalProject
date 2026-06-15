namespace OnlineAuctionApp.Domain.Entities;
using OnlineAuctionApp.Domain.Common;
using OnlineAuctionApp.Domain.Enums;
public class RefreshToken : BaseEntity
{
    public string Token { get; set; } = null!;
    public DateTime Expires { get; set; }
    public bool IsRevoked { get; set; }
    public Guid UserId { get; set; }
    public AppUser User { get; set; } = null!;
}