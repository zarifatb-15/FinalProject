using Microsoft.AspNetCore.Identity;

namespace OnlineAuctionApp.Domain.Entities;

public class AppUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}