using Hephaestus.Repository.Abstraction.Contract;
using Hephaestus.Repository.Abstraction.EventProcessing.DomainEvent;
using Hephaestus.Repository.Elasticsearch.Configure;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SampleWebApiApplicationWithElasticsearch.Persistence;
using SampleWebApiApplicationWithElasticsearch.Persistence.EventProcessing;

namespace SampleWebApiApplicationWithElasticsearch.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var elasticsearchConfig = configuration.GetSection("ConnectionStrings:Elastic").Value;
            var config = new ElasticsearchConfig()
            {
                ConnectionString = elasticsearchConfig
            };

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();

            services.AddMediatR(typeof(Startup).Assembly);
            services.AddElasticSearchClient(config);
            return services;
        }

        public static IServiceCollection AddElasticSearchClient(this IServiceCollection services, ElasticsearchConfig config)
        {
            services.AddSingleton<SampleElasticDbContext>(provider =>
            {
                var context = new SampleElasticDbContext(Options.Create<ElasticsearchConfig>(config));
                context.Configure();
                return context;
            });

            return services;
        }
    }
}
