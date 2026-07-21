using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineAuctionApp.Application.Common.Exceptions;
using OnlineAuctionApp.Application.DTOs.Admin;
using OnlineAuctionApp.Application.Interfaces.Services;
using OnlineAuctionApp.Domain.Entities;
using OnlineAuctionApp.Domain.Enums;
using OnlineAuctionApp.Persistence.Contexts;

namespace OnlineAuctionApp.Persistence.Services;

public class AdminService : IAdminService
{
    private readonly AppDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public AdminService(AppDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<AdminDashboardDto> GetDashboardAsync()
    {
        var recentUsersSource = await _userManager.Users
            .OrderBy(user => user.UserName)
            .Take(8)
            .ToListAsync();

        var recentUsers = new List<AdminUserReturnDto>();

        foreach (var user in recentUsersSource)
        {
            recentUsers.Add(await MapUserAsync(user));
        }

        var recentAuctionsSource = await _context.Auctions
            .AsNoTracking()
            .Include(auction => auction.Category)
            .Include(auction => auction.Seller)
            .Include(auction => auction.Winner)
            .Include(auction => auction.Bids)
            .OrderByDescending(auction => auction.CreatedDate)
            .Take(8)
            .ToListAsync();

        var allUsers = await _userManager.Users.ToListAsync();

        var buyerCount = 0;
        var sellerCount = 0;
        var adminCount = 0;

        foreach (var user in allUsers)
        {
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Buyer"))
            {
                buyerCount++;
            }

            if (roles.Contains("Seller"))
            {
                sellerCount++;
            }

            if (roles.Contains("Admin"))
            {
                adminCount++;
            }
        }

        var summary = new AdminDashboardSummaryDto
        {
            TotalUsers = allUsers.Count,
            BuyerCount = buyerCount,
            SellerCount = sellerCount,
            AdminCount = adminCount,
            TotalAuctions = await _context.Auctions.CountAsync(),
            ActiveAuctions = await _context.Auctions.CountAsync(
                auction => auction.Status == AuctionStatus.Active),
            CompletedAuctions = await _context.Auctions.CountAsync(
                auction => auction.Status == AuctionStatus.Completed),
            CancelledAuctions = await _context.Auctions.CountAsync(
                auction => auction.Status == AuctionStatus.Cancelled),
            TotalBids = await _context.Bids.CountAsync(),
            TotalCategories = await _context.Categories.CountAsync()
        };

        return new AdminDashboardDto
        {
            Summary = summary,
            RecentUsers = recentUsers,
            RecentAuctions = recentAuctionsSource
                .Select(MapAuction)
                .ToList()
        };
    }

    public async Task<List<AdminUserReturnDto>> GetUsersAsync()
    {
        var users = await _userManager.Users
            .OrderBy(user => user.UserName)
            .ToListAsync();

        var result = new List<AdminUserReturnDto>();

        foreach (var user in users)
        {
            result.Add(await MapUserAsync(user));
        }

        return result;
    }

    public async Task<List<AdminAuctionReturnDto>> GetAuctionsAsync()
    {
        var auctions = await _context.Auctions
            .AsNoTracking()
            .Include(auction => auction.Category)
            .Include(auction => auction.Seller)
            .Include(auction => auction.Winner)
            .Include(auction => auction.Bids)
            .OrderByDescending(auction => auction.CreatedDate)
            .ToListAsync();

        return auctions
            .Select(MapAuction)
            .ToList();
    }

    public async Task<AdminAuctionReturnDto> CancelAuctionAsync(Guid auctionId)
    {
        var auction = await _context.Auctions
            .Include(auction => auction.Category)
            .Include(auction => auction.Seller)
            .Include(auction => auction.Winner)
            .Include(auction => auction.Bids)
            .FirstOrDefaultAsync(auction => auction.Id == auctionId);

        if (auction is null)
        {
            throw new NotFoundException("Auction not found.");
        }

        if (auction.Status != AuctionStatus.Active)
        {
            throw new ConflictException("Only active auctions can be cancelled.");
        }

        auction.Status = AuctionStatus.Cancelled;
        auction.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapAuction(auction);
    }

    private async Task<AdminUserReturnDto> MapUserAsync(AppUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        return new AdminUserReturnDto
        {
            Id = user.Id,
            Username = user.UserName ?? "Unknown",
            Email = user.Email ?? "No email",
            FullName = $"{user.FirstName} {user.LastName}".Trim(),
            Roles = roles.ToList()
        };
    }

    private static AdminAuctionReturnDto MapAuction(Auction auction)
    {
        return new AdminAuctionReturnDto
        {
            Id = auction.Id,
            Title = auction.Title,
            CategoryName = auction.Category.Name,
            SellerUsername = auction.Seller.UserName ?? "Unknown seller",
            WinnerUsername = auction.Winner?.UserName,
            CurrentPrice = auction.CurrentPrice,
            Status = auction.Status.ToString(),
            EndTime = auction.EndTime,
            BidCount = auction.Bids.Count
        };
    }
}