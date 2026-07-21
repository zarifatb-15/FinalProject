using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineAuctionApp.Application.Interfaces.Services;
using OnlineAuctionApp.WebAPI.Extensions;

namespace OnlineAuctionApp.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController : BaseApiController
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyNotifications()
    {
        if (!User.TryGetUserId(out var userId))
        {
            return ApiUnauthorized("User is not authenticated.");
        }

        var notifications = await _notificationService.GetByUserIdAsync(userId);
        return ApiSuccess(notifications);
    }

    [HttpPatch("{notificationId}/read")]
    public async Task<IActionResult> MarkAsRead(Guid notificationId)
    {
        if (!User.TryGetUserId(out var userId))
        {
            return ApiUnauthorized("User is not authenticated.");
        }

        var notification = await _notificationService.MarkAsReadAsync(
            notificationId,
            userId);

        return ApiSuccess(notification);
    }
}