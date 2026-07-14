using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineAuctionApp.Application.DTOs.Auctions;
using OnlineAuctionApp.Application.DTOs.Bids;
using OnlineAuctionApp.Application.Interfaces.Services;
using OnlineAuctionApp.WebAPI.Extensions;
using OnlineAuctionApp.WebAPI.Requests;

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
    public async Task<IActionResult> GetAll([FromQuery] AuctionFilterDto filter)
    {
        try
        {
            var auctions = await _auctionService.GetAllAsync(filter);
            return Ok(auctions);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
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
        if (!User.TryGetUserId(out var sellerId))
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
        if (!User.TryGetUserId(out var sellerId))
            return Unauthorized(new { message = "Invalid user token." });

        var auctions = await _auctionService.GetBySellerIdAsync(sellerId);
        return Ok(auctions);
    }

    [Authorize(Roles = "Seller")]
    [HttpGet("seller/active")]
    public async Task<IActionResult> GetMyActiveAuctions()
    {
        if (!User.TryGetUserId(out var sellerId))
            return Unauthorized(new { message = "Invalid user token." });

        var auctions = await _auctionService.GetActiveBySellerIdAsync(sellerId);
        return Ok(auctions);
    }

    [Authorize(Roles = "Seller")]
    [HttpGet("seller/completed")]
    public async Task<IActionResult> GetMyCompletedAuctions()
    {
        if (!User.TryGetUserId(out var sellerId))
            return Unauthorized(new { message = "Invalid user token." });

        var auctions = await _auctionService.GetCompletedBySellerIdAsync(sellerId);
        return Ok(auctions);
    }

    [Authorize(Roles = "Seller")]
    [HttpGet("seller/dashboard-summary")]
    public async Task<IActionResult> GetSellerDashboardSummary()
    {
        if (!User.TryGetUserId(out var sellerId))
            return Unauthorized(new { message = "Invalid user token." });

        var summary = await _auctionService.GetSellerDashboardSummaryAsync(sellerId);
        return Ok(summary);
    }

    [Authorize(Roles = "Seller")]
    [HttpPost("{auctionId}/images")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadImage(Guid auctionId, [FromForm] AuctionImageUploadRequest request)
    {
        if (!User.TryGetUserId(out var sellerId))
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
        if (!User.TryGetUserId(out var buyerId))
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