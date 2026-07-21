using FluentValidation;
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
public class AuctionsController : BaseApiController
{
    private readonly IAuctionService _auctionService;
    private readonly IAuctionImageService _auctionImageService;
    private readonly IBidService _bidService;
    private readonly IAuctionClosingService _auctionClosingService;
    private readonly IValidator<AuctionCreateDto> _auctionCreateValidator;
    private readonly IValidator<AuctionUpdateDto> _auctionUpdateValidator;
    private readonly IValidator<BidCreateDto> _bidCreateValidator;

    public AuctionsController(
        IAuctionService auctionService,
        IAuctionImageService auctionImageService,
        IBidService bidService,
        IAuctionClosingService auctionClosingService,
        IValidator<AuctionCreateDto> auctionCreateValidator,
        IValidator<AuctionUpdateDto> auctionUpdateValidator,
        IValidator<BidCreateDto> bidCreateValidator)
    {
        _auctionService = auctionService;
        _auctionImageService = auctionImageService;
        _bidService = bidService;
        _auctionClosingService = auctionClosingService;
        _auctionCreateValidator = auctionCreateValidator;
        _auctionUpdateValidator = auctionUpdateValidator;
        _bidCreateValidator = bidCreateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] AuctionFilterDto filter)
    {
        var auctions = await _auctionService.GetAllAsync(filter);
        return ApiSuccess(auctions);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var auction = await _auctionService.GetByIdAsync(id);
        return ApiSuccess(auction);
    }

    [Authorize(Roles = "Seller")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AuctionCreateDto dto)
    {
        var authError = GetCurrentUserIdOrUnauthorized(out var sellerId);

        if (authError is not null)
        {
            return authError;
        }

        var validationError = await ValidateRequestAsync(
            dto,
            _auctionCreateValidator);

        if (validationError is not null)
        {
            return validationError;
        }

        var auction = await _auctionService.CreateAsync(dto, sellerId);
        return ApiCreated(nameof(GetById), new { id = auction.Id }, auction);
    }
    [Authorize(Roles = "Seller")]
    [HttpPut("{auctionId}")]
    public async Task<IActionResult> Update(
    Guid auctionId,
    [FromBody] AuctionUpdateDto dto)
    {
        var authError = GetCurrentUserIdOrUnauthorized(out var sellerId);

        if (authError is not null)
        {
            return authError;
        }

        var validationError = await ValidateRequestAsync(
            dto,
            _auctionUpdateValidator);

        if (validationError is not null)
        {
            return validationError;
        }

        var auction = await _auctionService.UpdateAsync(
            auctionId,
            dto,
            sellerId);

        return ApiSuccess(auction);
    }

    [Authorize(Roles = "Seller")]
    [HttpPatch("{auctionId}/cancel")]
    public async Task<IActionResult> CancelBySeller(Guid auctionId)
    {
        var authError = GetCurrentUserIdOrUnauthorized(out var sellerId);

        if (authError is not null)
        {
            return authError;
        }

        var auction = await _auctionService.CancelBySellerAsync(
            auctionId,
            sellerId);

        return ApiSuccess(auction);
    }

    [Authorize(Roles = "Seller")]
    [HttpGet("seller/my")]
    public async Task<IActionResult> GetMyAuctions()
    {
        var authError = GetCurrentUserIdOrUnauthorized(out var sellerId);

        if (authError is not null)
        {
            return authError;
        }

        var auctions = await _auctionService.GetBySellerIdAsync(sellerId);
        return ApiSuccess(auctions);
    }

    [Authorize(Roles = "Seller")]
    [HttpGet("seller/active")]
    public async Task<IActionResult> GetMyActiveAuctions()
    {
        var authError = GetCurrentUserIdOrUnauthorized(out var sellerId);

        if (authError is not null)
        {
            return authError;
        }

        var auctions = await _auctionService.GetActiveBySellerIdAsync(sellerId);
        return ApiSuccess(auctions);
    }

    [Authorize(Roles = "Seller")]
    [HttpGet("seller/completed")]
    public async Task<IActionResult> GetMyCompletedAuctions()
    {
        var authError = GetCurrentUserIdOrUnauthorized(out var sellerId);

        if (authError is not null)
        {
            return authError;
        }

        var auctions = await _auctionService.GetCompletedBySellerIdAsync(sellerId);
        return ApiSuccess(auctions);
    }

    [Authorize(Roles = "Seller")]
    [HttpGet("seller/dashboard-summary")]
    public async Task<IActionResult> GetSellerDashboardSummary()
    {
        var authError = GetCurrentUserIdOrUnauthorized(out var sellerId);

        if (authError is not null)
        {
            return authError;
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
        var authError = GetCurrentUserIdOrUnauthorized(out var sellerId);

        if (authError is not null)
        {
            return authError;
        }

        if (request.Image is null || request.Image.Length == 0)
        {
            return ApiBadRequest("Image is required.");
        }

        await using var stream = request.Image.OpenReadStream();

        var image = await _auctionImageService.AddImageAsync(
            auctionId,
            stream,
            request.Image.FileName,
            sellerId);

        return ApiSuccess(image);
    }

    [Authorize(Roles = "Buyer")]
    [HttpPost("{auctionId}/bids")]
    public async Task<IActionResult> PlaceBid(
        Guid auctionId,
        [FromBody] BidCreateDto dto)
    {
        var authError = GetCurrentUserIdOrUnauthorized(out var buyerId);

        if (authError is not null)
        {
            return authError;
        }

        var validationError = await ValidateRequestAsync(
            dto,
            _bidCreateValidator);

        if (validationError is not null)
        {
            return validationError;
        }

        var bid = await _bidService.PlaceBidAsync(auctionId, buyerId, dto);
        return ApiSuccess(bid);
    }

    [HttpGet("{auctionId}/bids")]
    public async Task<IActionResult> GetBidHistory(Guid auctionId)
    {
        var bids = await _bidService.GetByAuctionIdAsync(auctionId);
        return ApiSuccess(bids);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("close-expired")]
    public async Task<IActionResult> CloseExpiredAuctions()
    {
        var closedCount = await _auctionClosingService.CloseExpiredAuctionsAsync();

        return ApiSuccess(new
        {
            closedCount
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{auctionId}/close")]
    public async Task<IActionResult> CloseAuction(Guid auctionId)
    {
        var auction = await _auctionClosingService.CloseAuctionAsync(auctionId);
        return ApiSuccess(auction);
    }

    private IActionResult? GetCurrentUserIdOrUnauthorized(out Guid userId)
    {
        if (User.TryGetUserId(out userId))
        {
            return null;
        }

        return ApiUnauthorized("Invalid user token.");
    }
}