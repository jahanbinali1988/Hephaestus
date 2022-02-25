using Hephaestus.Repository.Abstraction.Base;
using Hephaestus.Repository.Abstraction.Contract;
using Hephaestus.Repository.Abstraction.Shared;
using Hephaestus.Repository.Elasticsearch.Configure;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Polly;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Hephaestus.Repository.Elasticsearch.Provider
{
    internal class InsertProvider<T> : ICommandProvider<T> where T : Entity
    {
        private readonly ElasticsearchConfig _config;
        private readonly IObjectPool<HttpClient> _clientsObjectPool;
        internal InsertProvider(ElasticsearchConfig config)
        {
            _clientsObjectPool = new ObjectPool<HttpClient>(() => new HttpClient(), 30);
            _config = config;
        }

        public Task ExecuteAsync(EntityContextInfo<T> contextInfo, CancellationToken token)
        {
            return Polly.Policy
                .Handle<HttpRequestException>()
                .Or<TaskCanceledException>()
                .Or<AggregateException>()
                .RetryAsync(2)
                .ExecuteAsync(() => InsertAsync(contextInfo.Document, contextInfo.EntityType, token));
        }

        private async Task InsertAsync<TDocument>(TDocument document, Type documentType, CancellationToken token) where TDocument : Entity
        {
            var httpClient = _clientsObjectPool.Acquire();
            try
            {
                var requestUri = $"{_config.ConnectionString}{documentType.Name.ToLower()}/_doc/{document.Id}";
                var serializerSettings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                var serializedEntity = JsonConvert.SerializeObject(document, serializerSettings);
                var content = new StringContent(serializedEntity);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var createResponse = await httpClient.PutAsync(requestUri, content, token).ConfigureAwait(true);
                if (!createResponse.IsSuccessStatusCode)
                {
                    throw new HttpRequestException();
                }
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new AggregateException(ex.Message);
            }
            finally
            {
                _clientsObjectPool.Release(httpClient);
            }
        }
             
    }
}
