using OnlineAuctionApp.Application.DTOs.Auth;
using OnlineAuctionApp.Domain.Entities;

namespace OnlineAuctionApp.Application.Interfaces.Services;

public interface IJwtService
{
    Task<TokenResponseDto> GenerateTokenAsync(AppUser user);
}