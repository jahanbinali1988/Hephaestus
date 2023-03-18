using Hephaestus.Repository.Abstraction.Contract;
using Hephaestus.Repository.Abstraction.EventProcessing.DomainEvent;
using Hephaestus.Repository.MongoDB.Configure;
using Hephaestus.Repository.MongoDB.Contract;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SampleWebApiApplicationWithMongoDb.Persistence;
using SampleWebApiApplicationWithMongoDb.Persistence.EventProcessing;

namespace SampleWebApiApplicationWithMongoDb.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbConfig>(configuration.GetSection("MongoDb"));

            services.AddMediatR(typeof(Startup).Assembly);
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IMongoDbUnitOfWork, UnitOfWork>();
            services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
            services.AddMongoClient(configuration);

            return services;
        }
        public static IServiceCollection AddMongoClient(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoConfigSection = configuration.GetSection("MongoDb");

            var config = mongoConfigSection.Get<MongoDbConfig>();

            services.AddScoped<SampleMongoDbContext>(provider =>
            {
                var context = new SampleMongoDbContext(Options.Create<MongoDbConfig>(config));
                context.Configure();
                return context;
            });

            return services;
        }
    }
}
