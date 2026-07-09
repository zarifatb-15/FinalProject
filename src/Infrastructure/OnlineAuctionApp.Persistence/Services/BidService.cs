using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineAuctionApp.Application.DTOs.Bids;
using OnlineAuctionApp.Application.Interfaces.Services;
using OnlineAuctionApp.Domain.Entities;
using OnlineAuctionApp.Domain.Enums;
using OnlineAuctionApp.Persistence.Contexts;

namespace OnlineAuctionApp.Persistence.Services;

public class BidService : IBidService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public BidService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BidReturnDto> PlaceBidAsync(Guid auctionId, Guid buyerId, BidCreateDto dto)
    {
        var auction = await _context.Auctions
            .FirstOrDefaultAsync(auction => auction.Id == auctionId);

        if (auction is null)
            throw new InvalidOperationException("Auction not found.");

        if (auction.Status != AuctionStatus.Active)
            throw new InvalidOperationException("Auction is not active.");

        if (auction.EndTime <= DateTime.UtcNow)
            throw new InvalidOperationException("Auction has already ended.");

        if (auction.SellerId == buyerId)
            throw new InvalidOperationException("Seller cannot bid on their own auction.");

        if (dto.Amount <= auction.CurrentPrice)
            throw new InvalidOperationException("Bid amount must be greater than current price.");

        var bid = new Bid
        {
            AuctionId = auction.Id,
            BuyerId = buyerId,
            Amount = dto.Amount,
            BidTime = DateTime.UtcNow
        };

        auction.CurrentPrice = dto.Amount;
        auction.UpdatedDate = DateTime.UtcNow;

        await _context.Bids.AddAsync(bid);
        await _context.SaveChangesAsync();

        var createdBid = await _context.Bids
            .AsNoTracking()
            .Include(bid => bid.Buyer)
            .FirstAsync(b => b.Id == bid.Id);

        return _mapper.Map<BidReturnDto>(createdBid);
    }

    public async Task<List<BidReturnDto>> GetByAuctionIdAsync(Guid auctionId)
    {
        var auctionExists = await _context.Auctions
            .AnyAsync(auction => auction.Id == auctionId);

        if (!auctionExists)
            throw new InvalidOperationException("Auction not found.");

        var bids = await _context.Bids
            .AsNoTracking()
            .Include(bid => bid.Buyer)
            .Where(bid => bid.AuctionId == auctionId)
            .OrderByDescending(bid => bid.BidTime)
            .ToListAsync();

        return _mapper.Map<List<BidReturnDto>>(bids);
    }
}