using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineAuctionApp.WebAPI.Constants;
using OnlineAuctionApp.Application.DTOs.Auth;
using FluentValidation;
using OnlineAuctionApp.Application.Interfaces.Services;

namespace OnlineAuctionApp.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;
    private readonly IValidator<RegisterDto> _registerValidator;
    private readonly IValidator<LoginDto> _loginValidator;

    public AuthController(IAuthService authService,
        IValidator<RegisterDto> registerValidator,
        IValidator<LoginDto> loginValidator)
    {
        _authService = authService;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var validationError = await ValidateRequestAsync(dto, _registerValidator);

        if (validationError is not null)
        {
            return validationError;
        }
        try
        {
            var token = await _authService.RegisterAsync(dto);
            SetAccessTokenCookie(token);

            return ApiSuccess(new
            {
                message = "Registered successfully.",
                expiration = token.Expiration
            });
        }
        catch (InvalidOperationException ex)
        {
            return ApiBadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var validationError = await ValidateRequestAsync(dto, _loginValidator);

        if (validationError is not null)
        {
            return validationError;
        }
        try
        {
            var token = await _authService.LoginAsync(dto);
            SetAccessTokenCookie(token);

            return ApiSuccess(new
            {
                message = "Logged in successfully.",
                expiration = token.Expiration
            });
        }
        catch (InvalidOperationException ex)
        {
            return ApiUnauthorized(ex.Message);
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

        return ApiSuccess(new
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
        Response.Cookies.Delete(AuthCookieNames.AccessToken, new CookieOptions
        {
            Path = "/"
        });

        return ApiSuccess(new
        {
            message = "Logged out successfully."
        });
    }
}