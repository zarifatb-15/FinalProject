using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineAuctionApp.Application.DTOs.Auctions;
using OnlineAuctionApp.Application.DTOs.Bids;
using OnlineAuctionApp.Application.Interfaces.Services;
using OnlineAuctionApp.WebAPI.Extensions;
using FluentValidation;
using OnlineAuctionApp.WebAPI.Requests;

namespace OnlineAuctionApp.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuctionsController : BaseApiController
{
    private readonly IAuctionService _auctionService;
    private readonly IAuctionImageService _auctionImageService;
    private readonly IBidService _bidService;
    private readonly IAuctionClosingService _auctionClosingService;
    private readonly IValidator<AuctionCreateDto> _auctionCreateValidator;
    private readonly IValidator<BidCreateDto> _bidCreateValidator;

    public AuctionsController(
        IAuctionService auctionService,
        IAuctionImageService auctionImageService,
        IBidService bidService,
        IAuctionClosingService auctionClosingService,
        IValidator<AuctionCreateDto> auctionCreateValidator,
        IValidator<BidCreateDto> bidCreateValidator)
    {
        _auctionService = auctionService;
        _auctionImageService = auctionImageService;
        _bidService = bidService;
        _auctionClosingService = auctionClosingService;
        _auctionCreateValidator = auctionCreateValidator;
        _bidCreateValidator = bidCreateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] AuctionFilterDto filter)
    {
        try
        {
            var auctions = await _auctionService.GetAllAsync(filter);
            return ApiSuccess(auctions);
        }
        catch (InvalidOperationException ex)
        {
            return ApiBadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var auction = await _auctionService.GetByIdAsync(id);
            return ApiSuccess(auction);
        }
        catch (InvalidOperationException ex)
        {
            return ApiNotFound(ex.Message);
        }
    }

    [Authorize(Roles = "Seller")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AuctionCreateDto dto)
    {
        if (!User.TryGetUserId(out var sellerId))
        {
            return ApiUnauthorized("Invalid user token.");
        }
        var validationError = await ValidateRequestAsync(dto, _auctionCreateValidator);

        if (validationError is not null)
        {
            return validationError;
        }
        try
        {
            var auction = await _auctionService.CreateAsync(dto, sellerId);
            return ApiCreated(nameof(GetById), new { id = auction.Id }, auction);
        }
        catch (InvalidOperationException ex)
        {
            return ApiBadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Seller")]
    [HttpGet("seller/my")]
    public async Task<IActionResult> GetMyAuctions()
    {
        if (!User.TryGetUserId(out var sellerId))
        {
            return ApiUnauthorized("Invalid user token.");
        }

        var auctions = await _auctionService.GetBySellerIdAsync(sellerId);
        return ApiSuccess(auctions);
    }

    [Authorize(Roles = "Seller")]
    [HttpGet("seller/active")]
    public async Task<IActionResult> GetMyActiveAuctions()
    {
        if (!User.TryGetUserId(out var sellerId))
        {
            return ApiUnauthorized("Invalid user token.");
        }

        var auctions = await _auctionService.GetActiveBySellerIdAsync(sellerId);
        return ApiSuccess(auctions);
    }

    [Authorize(Roles = "Seller")]
    [HttpGet("seller/completed")]
    public async Task<IActionResult> GetMyCompletedAuctions()
    {
        if (!User.TryGetUserId(out var sellerId))
        {
            return ApiUnauthorized("Invalid user token.");
        }

        var auctions = await _auctionService.GetCompletedBySellerIdAsync(sellerId);
        return ApiSuccess(auctions);
    }

    [Authorize(Roles = "Seller")]
    [HttpGet("seller/dashboard-summary")]
    public async Task<IActionResult> GetSellerDashboardSummary()
    {
        if (!User.TryGetUserId(out var sellerId))
        {
            return ApiUnauthorized("Invalid user token.");
        }

        var summary = await _auctionService.GetSellerDashboardSummaryAsync(sellerId);
        return ApiSuccess(summary);
    }

    [Authorize(Roles = "Seller")]
    [HttpPost("{auctionId}/images")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadImage(
        Guid auctionId,
        [FromForm] AuctionImageUploadRequest request)
    {
        if (!User.TryGetUserId(out var sellerId))
        {
            return ApiUnauthorized("Invalid user token.");
        }

        if (request.Image is null || request.Image.Length == 0)
        {
            return ApiBadRequest("Image is required.");
        }

        try
        {
            await using var stream = request.Image.OpenReadStream();

            var image = await _auctionImageService.AddImageAsync(
                auctionId,
                stream,
                request.Image.FileName,
                sellerId);

            return ApiSuccess(image);
        }
        catch (UnauthorizedAccessException ex)
        {
            return ApiForbidden(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message == "Auction not found.")
            {
                return ApiNotFound(ex.Message);
            }

            return ApiBadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Buyer")]
    [HttpPost("{auctionId}/bids")]
    public async Task<IActionResult> PlaceBid(Guid auctionId, [FromBody] BidCreateDto dto)
    {
        if (!User.TryGetUserId(out var buyerId))
        {
            return ApiUnauthorized("Invalid user token.");
        }
        var validationError = await ValidateRequestAsync(dto, _bidCreateValidator);

        if (validationError is not null)
        {
            return validationError;
        }

        try
        {
            var bid = await _bidService.PlaceBidAsync(auctionId, buyerId, dto);
            return ApiSuccess(bid);
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message == "Auction not found.")
            {
                return ApiNotFound(ex.Message);
            }

            return ApiBadRequest(ex.Message);
        }
    }

    [HttpGet("{auctionId}/bids")]
    public async Task<IActionResult> GetBidHistory(Guid auctionId)
    {
        try
        {
            var bids = await _bidService.GetByAuctionIdAsync(auctionId);
            return ApiSuccess(bids);
        }
        catch (InvalidOperationException ex)
        {
            return ApiNotFound(ex.Message);
        }
    }

    [Authorize]
    [HttpPost("close-expired")]
    public async Task<IActionResult> CloseExpiredAuctions()
    {
        var closedCount = await _auctionClosingService.CloseExpiredAuctionsAsync();

        return ApiSuccess(new
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
            return ApiSuccess(auction);
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message == "Auction not found.")
            {
                return ApiNotFound(ex.Message);
            }

            return ApiBadRequest(ex.Message);
        }
    }
}