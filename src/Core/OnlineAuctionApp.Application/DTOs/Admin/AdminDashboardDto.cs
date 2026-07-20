namespace OnlineAuctionApp.Application.DTOs.Admin;

public class AdminDashboardDto
{
    public AdminDashboardSummaryDto Summary { get; set; } = new();
    public List<AdminUserReturnDto> RecentUsers { get; set; } = new();
    public List<AdminAuctionReturnDto> RecentAuctions { get; set; } = new();
}