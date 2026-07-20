namespace OnlineAuctionApp.Application.DTOs.Admin;

public class AdminUserReturnDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public List<string> Roles { get; set; } = new();
}