using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineAuctionApp.Application.Common.Exceptions;
using OnlineAuctionApp.Application.DTOs.Auctions;
using OnlineAuctionApp.Application.Interfaces.Services;
using OnlineAuctionApp.Domain.Enums;
using OnlineAuctionApp.Persistence.Contexts;

namespace OnlineAuctionApp.Persistence.Services;

public class AuctionClosingService : IAuctionClosingService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly INotificationService _notificationService;
    private readonly IRealtimeNotificationService _realtimeNotificationService;

    public AuctionClosingService(
        AppDbContext context,
        IMapper mapper,
        INotificationService notificationService,
        IRealtimeNotificationService realtimeNotificationService)
    {
        _context = context;
        _mapper = mapper;
        _notificationService = notificationService;
        _realtimeNotificationService = realtimeNotificationService;
    }

    public async Task<int> CloseExpiredAuctionsAsync()
    {
        var expiredAuctionIds = await _context.Auctions
            .AsNoTracking()
            .Where(auction =>
                auction.Status == AuctionStatus.Active &&
                auction.EndTime <= DateTime.UtcNow)
            .Select(auction => auction.Id)
            .ToListAsync();

        var closedCount = 0;

        foreach (var auctionId in expiredAuctionIds)
        {
            await CloseAuctionAsync(auctionId);
            closedCount++;
        }

        return closedCount;
    }

    public async Task<AuctionReturnDto> CloseAuctionAsync(Guid auctionId)
    {
        var auction = await _context.Auctions
            .Include(auction => auction.Bids)
            .Include(auction => auction.Category)
            .Include(auction => auction.Images)
            .Include(auction => auction.Winner)
            .FirstOrDefaultAsync(auction => auction.Id == auctionId);

        if (auction is null)
        {
            throw new NotFoundException("Auction not found.");
        }

        if (auction.Status != AuctionStatus.Active)
        {
            throw new ConflictException("Auction is not active.");
        }

        if (auction.EndTime > DateTime.UtcNow)
        {
            throw new ConflictException("Auction has not ended yet.");
        }

        var highestBid = auction.Bids
            .OrderByDescending(bid => bid.Amount)
            .ThenBy(bid => bid.BidTime)
            .FirstOrDefault();

        auction.Status = AuctionStatus.Completed;
        auction.UpdatedDate = DateTime.UtcNow;

        if (highestBid is not null)
        {
            auction.WinnerId = highestBid.BuyerId;
            auction.CurrentPrice = highestBid.Amount;
        }

        await _context.SaveChangesAsync();

        if (highestBid is null)
        {
            await SendNotificationAsync(
                auction.SellerId,
                $"Your auction '{auction.Title}' has ended without any bids.");
        }
        else
        {
            await SendNotificationAsync(
                auction.SellerId,
                $"Your auction '{auction.Title}' has ended. Winning bid: {highestBid.Amount}.");

            await SendNotificationAsync(
                highestBid.BuyerId,
                $"Congratulations! You won auction '{auction.Title}'.");

            var loserIds = auction.Bids
                .Where(bid => bid.BuyerId != highestBid.BuyerId)
                .Select(bid => bid.BuyerId)
                .Distinct()
                .ToList();

            foreach (var loserId in loserIds)
            {
                await SendNotificationAsync(
                    loserId,
                    $"Auction '{auction.Title}' has ended. You did not win this auction.");
            }
        }

        var closedAuction = await _context.Auctions
            .AsNoTracking()
            .Include(auction => auction.Category)
            .Include(auction => auction.Images)
            .Include(auction => auction.Winner)
            .FirstAsync(auction => auction.Id == auctionId);

        return _mapper.Map<AuctionReturnDto>(closedAuction);
    }

    private async Task SendNotificationAsync(Guid userId, string message)
    {
        var notification = await _notificationService.CreateAsync(userId, message);

        await _realtimeNotificationService.SendNotificationAsync(
            userId,
            notification);
    }
}