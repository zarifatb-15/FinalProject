using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        var users = await _userManager.Users
        .OrderBy(user => user.UserName)
        .Take(8)
        .ToListAsync();

        var recentUsers = new List<AdminUserReturnDto>();

        foreach (var user in users)
        {
            recentUsers.Add(await MapUserAsync(user));
        }

        var recentAuctions = await _context.Auctions
            .AsNoTracking()
            .Include(auction => auction.Category)
            .Include(auction => auction.Seller)
            .Include(auction => auction.Winner)
            .Include(auction => auction.Bids)
            .OrderByDescending(auction => auction.CreatedDate)
            .Take(8)
            .Select(auction => new AdminAuctionReturnDto
            {
                Id = auction.Id,
                Title = auction.Title,
                CategoryName = auction.Category.Name,
                SellerUsername = auction.Seller.UserName ?? "Unknown seller",
                WinnerUsername = auction.Winner != null ? auction.Winner.UserName : null,
                CurrentPrice = auction.CurrentPrice,
                Status = auction.Status.ToString(),
                EndTime = auction.EndTime,
                BidCount = auction.Bids.Count
            })
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
            ActiveAuctions = await _context.Auctions.CountAsync(auction => auction.Status == AuctionStatus.Active),
            CompletedAuctions = await _context.Auctions.CountAsync(auction => auction.Status == AuctionStatus.Completed),
            CancelledAuctions = await _context.Auctions.CountAsync(auction => auction.Status == AuctionStatus.Cancelled),
            TotalBids = await _context.Bids.CountAsync(),
            TotalCategories = await _context.Categories.CountAsync()
        };

        return new AdminDashboardDto
        {
            Summary = summary,
            RecentUsers = recentUsers,
            RecentAuctions = recentAuctions
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
        return await _context.Auctions
            .AsNoTracking()
            .Include(auction => auction.Category)
            .Include(auction => auction.Seller)
            .Include(auction => auction.Winner)
            .Include(auction => auction.Bids)
            .OrderByDescending(auction => auction.CreatedDate)
            .Select(auction => new AdminAuctionReturnDto
            {
                Id = auction.Id,
                Title = auction.Title,
                CategoryName = auction.Category.Name,
                SellerUsername = auction.Seller.UserName ?? "Unknown seller",
                WinnerUsername = auction.Winner != null ? auction.Winner.UserName : null,
                CurrentPrice = auction.CurrentPrice,
                Status = auction.Status.ToString(),
                EndTime = auction.EndTime,
                BidCount = auction.Bids.Count
            })
            .ToListAsync();
    }

    public async Task<AdminAuctionReturnDto> CancelAuctionAsync(Guid auctionId)
    {
        var auction = await _context.Auctions
            .Include(item => item.Category)
            .Include(item => item.Seller)
            .Include(item => item.Winner)
            .Include(item => item.Bids)
            .FirstOrDefaultAsync(item => item.Id == auctionId);

        if (auction is null)
        {
            throw new InvalidOperationException("Auction not found.");
        }

        if (auction.Status != AuctionStatus.Active)
        {
            throw new InvalidOperationException("Only active auctions can be cancelled.");
        }

        auction.Status = AuctionStatus.Cancelled;
        auction.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

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
}