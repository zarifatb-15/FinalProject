using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineAuctionApp.Application.DTOs.Auctions;
using OnlineAuctionApp.Application.Interfaces.Services;
using OnlineAuctionApp.Domain.Entities;
using OnlineAuctionApp.Domain.Enums;
using OnlineAuctionApp.Persistence.Contexts;

namespace OnlineAuctionApp.Persistence.Services;

public class AuctionService : IAuctionService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public AuctionService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AuctionReturnDto>> GetAllAsync()
    {
        var auctions = await _context.Auctions
            .AsNoTracking()
            .Include(auction => auction.Category)
            .Include(auction => auction.Images)
            .Include(auction => auction.Winner)
            .OrderByDescending(auction => auction.CreatedDate)
            .ToListAsync();

        return _mapper.Map<List<AuctionReturnDto>>(auctions);
    }

    public async Task<AuctionReturnDto> GetByIdAsync(Guid id)
    {
        var auction = await _context.Auctions
            .AsNoTracking()
            .Include(auction => auction.Category)
            .Include(auction => auction.Images)
            .Include(auction => auction.Winner)
            .FirstOrDefaultAsync(auction => auction.Id == id);

        if (auction is null)
            throw new InvalidOperationException("Auction not found.");

        return _mapper.Map<AuctionReturnDto>(auction);
    }
    public async Task<List<AuctionReturnDto>> GetActiveBySellerIdAsync(Guid sellerId)
    {
        var auctions = await _context.Auctions
            .AsNoTracking()
            .Include(auction => auction.Category)
            .Include(auction => auction.Images)
            .Include(auction => auction.Winner)
            .Where(auction =>
                auction.SellerId == sellerId &&
                auction.Status == AuctionStatus.Active)
            .OrderByDescending(auction => auction.CreatedDate)
            .ToListAsync();

        return _mapper.Map<List<AuctionReturnDto>>(auctions);
    }

    public async Task<List<AuctionReturnDto>> GetCompletedBySellerIdAsync(Guid sellerId)
    {
        var auctions = await _context.Auctions
            .AsNoTracking()
            .Include(auction => auction.Category)
            .Include(auction => auction.Images)
            .Include(auction => auction.Winner)
            .Where(auction =>
                auction.SellerId == sellerId &&
                auction.Status == AuctionStatus.Completed)
            .OrderByDescending(auction => auction.CreatedDate)
            .ToListAsync();

        return _mapper.Map<List<AuctionReturnDto>>(auctions);
    }
    public async Task<SellerDashboardSummaryDto> GetSellerDashboardSummaryAsync(Guid sellerId)
    {
        var activeAuctionCount = await _context.Auctions
            .CountAsync(auction =>
                auction.SellerId == sellerId &&
                auction.Status == AuctionStatus.Active);

        var completedAuctionCount = await _context.Auctions
            .CountAsync(auction =>
                auction.SellerId == sellerId &&
                auction.Status == AuctionStatus.Completed);

        var totalAuctionCount = await _context.Auctions
            .CountAsync(auction => auction.SellerId == sellerId);

        return new SellerDashboardSummaryDto
        {
            ActiveAuctionCount = activeAuctionCount,
            CompletedAuctionCount = completedAuctionCount,
            TotalAuctionCount = totalAuctionCount
        };
    }

    public async Task<AuctionReturnDto> CreateAsync(AuctionCreateDto dto, Guid sellerId)
    {
        if (dto.StartingPrice <= 0)
            throw new InvalidOperationException("Starting price must be greater than zero.");

        if (dto.EndTime <= DateTime.UtcNow)
            throw new InvalidOperationException("End time must be in the future.");

        var categoryExists = await _context.Categories
            .AnyAsync(category => category.Id == dto.CategoryId);

        if (!categoryExists)
            throw new InvalidOperationException("Category not found.");

        var auction = _mapper.Map<Auction>(dto);

        auction.SellerId = sellerId;
        auction.CurrentPrice = dto.StartingPrice;
        auction.Status = AuctionStatus.Active;

        await _context.Auctions.AddAsync(auction);
        await _context.SaveChangesAsync();

        var createdAuction = await _context.Auctions
            .AsNoTracking()
            .Include(a => a.Category)
            .Include(a => a.Images)
            .FirstAsync(a => a.Id == auction.Id);

        return _mapper.Map<AuctionReturnDto>(createdAuction);
    }

    public async Task<List<AuctionReturnDto>> GetBySellerIdAsync(Guid sellerId)
    {
        var auctions = await _context.Auctions
            .AsNoTracking()
            .Include(auction => auction.Category)
            .Include(auction => auction.Images)
            .Include(auction => auction.Winner)
            .Where(auction => auction.SellerId == sellerId)
            .OrderByDescending(auction => auction.CreatedDate)
            .ToListAsync();

        return _mapper.Map<List<AuctionReturnDto>>(auctions);
    }
}