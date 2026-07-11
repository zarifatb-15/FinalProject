using OnlineAuctionApp.Application.DTOs.Notifications;
namespace OnlineAuctionApp.Application.Interfaces.Services;
public interface IRealtimeNotificationService
{
    Task SendNotificationAsync(Guid userId, NotificationReturnDto notification);
}