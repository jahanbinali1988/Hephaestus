using System;
using Elasticsearch.Net;
using System.Linq;
using Nest;
using Microsoft.Extensions.DependencyInjection;
using Hephaestus.Repository.Elasticsearch.MappingConfiguration;
using Hephaestus.Repository.Abstraction.Exceptions;
using Hephaestus.Repository.Elasticsearch.Configure;
using Hephaestus.Repository.Abstraction.Base;

namespace Hephaestus.Repository.Elasticsearch.Extensions
{
    public static class ServiceCollectionExtensions
    {
//        public static IServiceCollection AddElasticSearchClient(this IServiceCollection services, ElasticsearchConfig config)
//        {
//            services.AddSingleton<IElasticClient>(provider =>
//            {
//                var uris = config.ConnectionString
//                    .Split(';').Select(x => new Uri(x));

//                var pool = new StaticConnectionPool(uris, randomize: false)
//                {
//                    SniffedOnStartup = true,
//                };

//                var connection = new ConnectionSettings(pool);
//#if DEBUG
//                connection.EnableDebugMode();
//#endif
//                var client = new ElasticClient(connection);
                
//                return client;
//            });

//            return services;
//        }

        public static IElasticClient ApplyMappingConfiguration<T>(this IElasticClient client, ConnectionSettings connection,
            IElasticMappingConfiguration<T> configuration) where T : Entity
        {
            connection.DefaultMappingFor<T>(x =>
                x.IndexName(configuration.IndexName)
                    .IdProperty(configuration.IdPropertyName));

            var response = client.LowLevel.Indices.GetMapping<GetMappingResponse>(configuration.IndexName, null);
            if (response == null || !response.IsValid)
            {

                var createResponse = client.Indices.Create(configuration.IndexName, c => c
                    .Index<T>()
                    .Map<T>(m => m.Properties(configuration.MapPropertiesDescriptor).Dynamic(false))
                    .Settings(s =>
                        s.NumberOfReplicas(configuration.NumberOfReplicas).NumberOfShards(configuration.NumberOfShards))
                );
                if (!createResponse.IsValid)
                    throw new InvalidOperationException("unable to create mapping");

            }
            return client;
        }

        public static void EnsureRequestSuccess(this IElasticsearchResponse response)
        {
            if (response.ApiCall?.HttpStatusCode == 404)
                throw new EntityNotFoundException("");

            if (response.ApiCall?.HttpStatusCode != 200 && response.ApiCall?.HttpStatusCode != 201)
            {
                if (response.ApiCall != null)
                {
                    var ex = new Exception(response.ApiCall?.OriginalException?.Message, response.ApiCall?.OriginalException);
                    if (response.ApiCall.OriginalException is ElasticsearchClientException elasticClientException)
                    {
                        ex.Data.Add("DebugInformation", elasticClientException.DebugInformation);
                        if (elasticClientException.FailureReason.HasValue)
                            ex.Data.Add("FailureReason", elasticClientException.FailureReason.Value);
                    }
                    throw ex;
                }
            }
        }
    }
}
