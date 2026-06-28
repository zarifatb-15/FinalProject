namespace OnlineAuctionApp.Application.DTOs.Auth;
public class TokenResponseDto
{
    public string AccessToken { get; set; } = null!;
    public DateTime Expiration { get; set; }
}