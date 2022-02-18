using Hephaestus.Repository.Abstraction.Base;
using Hephaestus.Repository.MongoDB.MappingConfiguration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Hephaestus.Repository.MongoDB.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IEnumerable<MongoTypeConfiguration<Entity>> configurations)
        {
            foreach (var configure in configurations)
            {
                configure.Configure();
            }
            services.AddScoped<IMongoContext, MongoContext>();

            return services;
        }
    }
}
