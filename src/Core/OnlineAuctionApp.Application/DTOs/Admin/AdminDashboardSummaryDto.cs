namespace OnlineAuctionApp.Application.DTOs.Admin;

public class AdminDashboardSummaryDto
{
    public int TotalUsers { get; set; }
    public int BuyerCount { get; set; }
    public int SellerCount { get; set; }
    public int AdminCount { get; set; }

    public int TotalAuctions { get; set; }
    public int ActiveAuctions { get; set; }
    public int CompletedAuctions { get; set; }
    public int CancelledAuctions { get; set; }

    public int TotalBids { get; set; }
    public int TotalCategories { get; set; }
}