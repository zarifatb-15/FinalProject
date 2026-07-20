using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineAuctionApp.Application.Interfaces.Services;

namespace OnlineAuctionApp.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : BaseApiController
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        var dashboard = await _adminService.GetDashboardAsync();
        return ApiSuccess(dashboard);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _adminService.GetUsersAsync();
        return ApiSuccess(users);
    }

    [HttpGet("auctions")]
    public async Task<IActionResult> GetAuctions()
    {
        var auctions = await _adminService.GetAuctionsAsync();
        return ApiSuccess(auctions);
    }

    [HttpPatch("auctions/{auctionId}/cancel")]
    public async Task<IActionResult> CancelAuction(Guid auctionId)
    {
        try
        {
            var auction = await _adminService.CancelAuctionAsync(auctionId);
            return ApiSuccess(auction);
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message == "Auction not found.")
            {
                return ApiNotFound(ex.Message);
            }

            return ApiConflict(ex.Message);
        }
    }
}