using Microsoft.AspNetCore.Identity;
using OnlineAuctionApp.Application.Interfaces.Services;
using OnlineAuctionApp.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineAuctionApp.Domain.Entities;
using OnlineAuctionApp.Persistence.Contexts;

namespace OnlineAuctionApp.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default")));

        services.AddIdentityCore<AppUser>(options =>
    {

        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;


        options.User.RequireUniqueEmail = true;
    })
    .AddRoles<AppRole>()
    .AddEntityFrameworkStores<AppDbContext>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IAuctionService, AuctionService>();
        services.AddScoped<IAuctionImageService, AuctionImageService>();
        services.AddScoped<IBidService, BidService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IAuctionClosingService, AuctionClosingService>();
        services.AddScoped<IAdminService, AdminService>();
        return services;
    }
}