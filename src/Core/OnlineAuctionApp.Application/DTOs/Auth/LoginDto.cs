namespace OnlineAuctionApp.Application.DTOs.Auth;

public class LoginDto
{
    public string UsernameOrEmail { get; set; } = null!;
    
    public string Password { get; set; } = null!;
}