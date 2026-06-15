namespace OnlineAuctionApp.Domain.Entities;
using OnlineAuctionApp.Domain.Common;
using OnlineAuctionApp.Domain.Enums;
public class AppUser : BaseEntity
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public UserRole Role { get; set; } 
}