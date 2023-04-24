using Hephaestus.Cache.Contracts;
using Hephaestus.Cache.Configure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hephaestus.Cache.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisService(this IServiceCollection services, IConfiguration configuration)
        {
            CacheConfig cacheConfigurations = new();
            configuration.GetSection("CacheConfigurations").Bind(cacheConfigurations);
            services.Configure<CacheConfig>(value => configuration.GetSection("CacheConfigurations"));

            services.AddStackExchangeRedisCache(options => { options.Configuration = cacheConfigurations.Url; });

            services.AddScoped(typeof(ICacheBaseRepository<>), typeof(CacheBaseRepository<>));

            return services;
        }
    }
}
