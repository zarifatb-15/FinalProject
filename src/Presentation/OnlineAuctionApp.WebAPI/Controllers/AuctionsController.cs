using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineAuctionApp.WebAPI.Requests;
using OnlineAuctionApp.Application.DTOs.Auctions;
using OnlineAuctionApp.Application.DTOs.Bids;
using OnlineAuctionApp.Application.Interfaces.Services;

namespace OnlineAuctionApp.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuctionsController : ControllerBase
{
    private readonly IAuctionService _auctionService;
    private readonly IAuctionImageService _auctionImageService;
    private readonly IBidService _bidService;
    private readonly IAuctionClosingService _auctionClosingService;

    public AuctionsController(IAuctionService auctionService,
        IAuctionImageService auctionImageService,
        IBidService bidService,
        IAuctionClosingService auctionClosingService)
    {
        _auctionImageService = auctionImageService;
        _auctionService = auctionService;
        _bidService = bidService;
        _auctionClosingService = auctionClosingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var auctions = await _auctionService.GetAllAsync();
        return Ok(auctions);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var auction = await _auctionService.GetByIdAsync(id);
            return Ok(auction);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "Seller")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AuctionCreateDto dto)
    {
        var sellerIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(sellerIdValue, out var sellerId))
            return Unauthorized(new { message = "Invalid user token." });

        try
        {
            var auction = await _auctionService.CreateAsync(dto, sellerId);
            return CreatedAtAction(nameof(GetById), new { id = auction.Id }, auction);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "Seller")]
    [HttpGet("seller/my")]
    public async Task<IActionResult> GetMyAuctions()
    {
        var sellerIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(sellerIdValue, out var sellerId))
            return Unauthorized(new { message = "Invalid user token." });

        var auctions = await _auctionService.GetBySellerIdAsync(sellerId);
        return Ok(auctions);
    }

    [Authorize(Roles = "Seller")]
    [HttpPost("{auctionId}/images")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadImage(Guid auctionId, [FromForm] AuctionImageUploadRequest request)
    {
        var sellerIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(sellerIdValue, out var sellerId))
            return Unauthorized(new { message = "Invalid user token." });

        if (request.Image is null || request.Image.Length == 0)
            return BadRequest(new { message = "Image is required." });

        try
        {
            await using var stream = request.Image.OpenReadStream();

            var image = await _auctionImageService.AddImageAsync(
                auctionId,
                stream,
                request.Image.FileName,
                sellerId);

            return Ok(image);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    [Authorize(Roles = "Buyer")]
    [HttpPost("{auctionId}/bids")]
    public async Task<IActionResult> PlaceBid(Guid auctionId, [FromBody] BidCreateDto dto)
    {
        var buyerIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(buyerIdValue, out var buyerId))
            return Unauthorized(new { message = "Invalid user token." });

        try
        {
            var bid = await _bidService.PlaceBidAsync(auctionId, buyerId, dto);
            return Ok(bid);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    [HttpGet("{auctionId}/bids")]
    public async Task<IActionResult> GetBidHistory(Guid auctionId)
    {
        try
        {
            var bids = await _bidService.GetByAuctionIdAsync(auctionId);
            return Ok(bids);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
    [Authorize]
    [HttpPost("close-expired")]
    public async Task<IActionResult> CloseExpiredAuctions()
    {
        var closedCount = await _auctionClosingService.CloseExpiredAuctionsAsync();

        return Ok(new
        {
            closedCount
        });
    }
    [Authorize]
    [HttpPost("{auctionId}/close")]
    public async Task<IActionResult> CloseAuction(Guid auctionId)
    {
        try
        {
            var auction = await _auctionClosingService.CloseAuctionAsync(auctionId);
            return Ok(auction);
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message == "Auction not found.")
                return NotFound(new { message = ex.Message });

            return BadRequest(new { message = ex.Message });
        }
    }
}