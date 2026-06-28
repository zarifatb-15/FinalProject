using OnlineAuctionApp.Application.DTOs.Auth;

namespace OnlineAuctionApp.Application.Interfaces.Services;

public interface IAuthService
{
    Task<TokenResponseDto> RegisterAsync(RegisterDto dto);
    Task<TokenResponseDto> LoginAsync(LoginDto dto);
}