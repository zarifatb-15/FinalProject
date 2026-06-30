using Microsoft.Extensions.DependencyInjection;
using OnlineAuctionApp.Application.Profiles;

namespace OnlineAuctionApp.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(MapperProfile));

        return services;
    }
}