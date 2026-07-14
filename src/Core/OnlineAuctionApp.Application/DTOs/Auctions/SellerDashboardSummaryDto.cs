namespace OnlineAuctionApp.Application.DTOs.Auctions;

public class SellerDashboardSummaryDto
{
    public int ActiveAuctionCount { get; set; }
    public int CompletedAuctionCount { get; set; }
    public int TotalAuctionCount { get; set; }
}