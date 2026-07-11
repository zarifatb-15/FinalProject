using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineAuctionApp.Application.DTOs.Notifications;
using OnlineAuctionApp.Application.Interfaces.Services;
using OnlineAuctionApp.Domain.Entities;
using OnlineAuctionApp.Persistence.Contexts;

namespace OnlineAuctionApp.Persistence.Services;

public class NotificationService : INotificationService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public NotificationService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<NotificationReturnDto> CreateAsync(Guid userId, string message)
    {
        var userExists = await _context.Users
            .AnyAsync(user => user.Id == userId);

        if (!userExists)
            throw new InvalidOperationException("User not found.");

        var notification = new Notification
        {
            UserId = userId,
            Message = message,
            IsRead = false
        };

        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();

        return _mapper.Map<NotificationReturnDto>(notification);
    }

    public async Task<List<NotificationReturnDto>> GetByUserIdAsync(Guid userId)
    {
        var notifications = await _context.Notifications
            .AsNoTracking()
            .Where(notification => notification.UserId == userId)
            .OrderByDescending(notification => notification.CreatedDate)
            .ToListAsync();

        return _mapper.Map<List<NotificationReturnDto>>(notifications);
    }

    public async Task<NotificationReturnDto> MarkAsReadAsync(Guid notificationId, Guid userId)
    {
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(notification =>
                notification.Id == notificationId &&
                notification.UserId == userId);

        if (notification is null)
            throw new InvalidOperationException("Notification not found.");

        notification.IsRead = true;
        notification.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return _mapper.Map<NotificationReturnDto>(notification);
    }
}