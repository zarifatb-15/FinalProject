using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineAuctionApp.WebAPI.Requests;
using OnlineAuctionApp.Application.DTOs.Auctions;
using OnlineAuctionApp.Application.Interfaces.Services;

namespace OnlineAuctionApp.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuctionsController : ControllerBase
{
    private readonly IAuctionService _auctionService;
    private readonly IAuctionImageService _auctionImageService;

    public AuctionsController(IAuctionService auctionService,
    IAuctionImageService auctionImageService)
    {
        _auctionImageService = auctionImageService;
        _auctionService = auctionService;
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
}