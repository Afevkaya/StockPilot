using Microsoft.Extensions.DependencyInjection;

namespace StockPilot.Persistence.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        // Register persistence services here
        return services;
    }
}
