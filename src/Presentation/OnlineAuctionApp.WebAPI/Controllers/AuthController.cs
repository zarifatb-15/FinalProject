using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineAuctionApp.WebAPI.Constants;
using OnlineAuctionApp.Application.DTOs.Auth;
using OnlineAuctionApp.Application.Interfaces.Services;

namespace OnlineAuctionApp.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        try
        {
            var token = await _authService.RegisterAsync(dto);

            SetAccessTokenCookie(token);

            return Ok(new
            {
                message = "Registered successfully.",
                expiration = token.Expiration
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var token = await _authService.LoginAsync(dto);

            SetAccessTokenCookie(token);

            return Ok(new
            {
                message = "Logged in successfully.",
                expiration = token.Expiration
            });
        }
        catch (InvalidOperationException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var username = User.FindFirstValue(ClaimTypes.Name);
        var email = User.FindFirstValue(ClaimTypes.Email);
        var roles = User.FindAll(ClaimTypes.Role)
            .Select(role => role.Value)
            .ToList();

        return Ok(new
        {
            userId,
            username,
            email,
            roles
        });
    }

    private void SetAccessTokenCookie(TokenResponseDto tokenResponse)
    {
        Response.Cookies.Append(
            AuthCookieNames.AccessToken,
            tokenResponse.AccessToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.Lax,
                Expires = new DateTimeOffset(tokenResponse.Expiration),
                Path = "/"
            });
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(
            AuthCookieNames.AccessToken,
            new CookieOptions
            {
                Path = "/"
            });

        return Ok(new { message = "Logged out successfully." });
    }
}