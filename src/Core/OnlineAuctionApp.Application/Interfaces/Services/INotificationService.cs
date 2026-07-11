using OnlineAuctionApp.Application.DTOs.Notifications;

namespace OnlineAuctionApp.Application.Interfaces.Services;

public interface INotificationService
{
    Task<NotificationReturnDto> CreateAsync(Guid userId, string message);

    Task<List<NotificationReturnDto>> GetByUserIdAsync(Guid userId);

    Task<NotificationReturnDto> MarkAsReadAsync(Guid notificationId, Guid userId);
}