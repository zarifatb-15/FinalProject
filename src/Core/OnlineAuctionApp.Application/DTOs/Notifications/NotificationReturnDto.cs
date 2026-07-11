namespace OnlineAuctionApp.Application.DTOs.Notifications;

public class NotificationReturnDto
{
    public Guid Id { get; set; }

    public string Message { get; set; } = null!;

    public Guid UserId { get; set; }

    public bool IsRead { get; set; }

    public DateTime CreatedDate { get; set; }
}