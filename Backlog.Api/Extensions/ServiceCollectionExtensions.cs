using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backlog.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBacklogApi(
        this IServiceCollection services,
        IConfiguration configurationSection)
    {
        services.Configure<BacklogClientOptions>(configurationSection);
        services.AddSingleton<BacklogClient>();

        return services;
    }
}