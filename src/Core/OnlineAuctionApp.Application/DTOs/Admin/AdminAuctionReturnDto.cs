namespace OnlineAuctionApp.Application.DTOs.Admin;

public class AdminAuctionReturnDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string CategoryName { get; set; } = null!;
    public string SellerUsername { get; set; } = null!;
    public string? WinnerUsername { get; set; }
    public decimal CurrentPrice { get; set; }
    public string Status { get; set; } = null!;
    public DateTime EndTime { get; set; }
    public int BidCount { get; set; }
}