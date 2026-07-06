using Microsoft.Extensions.DependencyInjection;
using OnlineAuctionApp.Application.Interfaces.Services;
using OnlineAuctionApp.Infrastructure.Services;

namespace OnlineAuctionApp.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IFileService, FileService>();

        return services;
    }
}