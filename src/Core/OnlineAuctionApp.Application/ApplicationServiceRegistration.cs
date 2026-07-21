using Microsoft.Extensions.DependencyInjection;
using OnlineAuctionApp.Application.Profiles;
using FluentValidation;

namespace OnlineAuctionApp.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(MapperProfile));
        services.AddValidatorsFromAssembly(typeof(ApplicationServiceRegistration).Assembly);

        return services;
    }
}