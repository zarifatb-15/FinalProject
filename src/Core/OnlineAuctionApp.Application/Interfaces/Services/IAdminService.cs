using OnlineAuctionApp.Application.DTOs.Admin;

namespace OnlineAuctionApp.Application.Interfaces.Services;

public interface IAdminService
{
    Task<AdminDashboardDto> GetDashboardAsync();
    Task<List<AdminUserReturnDto>> GetUsersAsync();
    Task<List<AdminAuctionReturnDto>> GetAuctionsAsync();
    Task<AdminAuctionReturnDto> CancelAuctionAsync(Guid auctionId);
}