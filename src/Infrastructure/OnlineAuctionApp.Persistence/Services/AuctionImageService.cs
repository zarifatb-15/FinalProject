using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineAuctionApp.Application.DTOs.Auctions;
using OnlineAuctionApp.Application.Interfaces.Services;
using OnlineAuctionApp.Domain.Entities;
using OnlineAuctionApp.Persistence.Contexts;

namespace OnlineAuctionApp.Persistence.Services;

public class AuctionImageService : IAuctionImageService
{
    private readonly AppDbContext _context;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;

    public AuctionImageService(AppDbContext context, IFileService fileService, IMapper mapper)
    {
        _context = context;
        _fileService = fileService;
        _mapper = mapper;
    }

    public async Task<AuctionImageReturnDto> AddImageAsync(
        Guid auctionId,
        Stream fileStream,
        string fileName,
        Guid sellerId)
    {
        var auction = await _context.Auctions
            .Include(auction => auction.Images)
            .FirstOrDefaultAsync(auction => auction.Id == auctionId);

        if (auction is null)
            throw new InvalidOperationException("Auction not found.");

        if (auction.SellerId != sellerId)
            throw new UnauthorizedAccessException("You can only upload images to your own auction.");

        var imageUrl = await _fileService.SaveFileAsync(fileStream, fileName, "auctions");

        var isFirstImage = !auction.Images.Any();

        var auctionImage = new AuctionImage
        {
            AuctionId = auction.Id,
            ImageUrl = imageUrl,
            IsPrimary = isFirstImage
        };

        await _context.AuctionImages.AddAsync(auctionImage);
        await _context.SaveChangesAsync();

        return _mapper.Map<AuctionImageReturnDto>(auctionImage);
    }
}