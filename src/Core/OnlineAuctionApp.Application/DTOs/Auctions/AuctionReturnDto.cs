namespace OnlineAuctionApp.Application.DTOs.Auctions;

public class AuctionReturnDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;

    public decimal StartingPrice { get; set; }
    public decimal CurrentPrice { get; set; }

    public DateTime EndTime { get; set; }

    public string Status { get; set; } = null!;

    public Guid SellerId { get; set; }

    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
}