namespace OnlineAuctionApp.Application.DTOs.Auctions;

public class AuctionCreateDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal StartingPrice { get; set; }
    public DateTime EndTime { get; set; }
    public Guid CategoryId { get; set; }
}