using Elasticsearch.Net;
using Hephaestus.Repository.Abstraction.Contract;
using Nest;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Hephaestus.Repository.Abstraction.Base;
using Hephaestus.Repository.Elasticsearch.Configure;
using Microsoft.Extensions.Options;
using Hephaestus.Repository.Elasticsearch.Provider;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Hephaestus.Repository.Elasticsearch
{
    public class ElasticDbContext : IDisposable
    {
        protected IElasticClient _elasticClient;
        protected ConnectionSettings _connectionSettings;
        private ConcurrentQueue<EntityContextInfo<Entity>> _entityPendingChanges;
        private ConcurrentQueue<IDomainEvent> _domainEvents;
        private readonly ElsticDbCommandDispatcher _commandDispatcher;
        private readonly ElasticsearchConfig _config;
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);
        public ElasticDbContext(IOptions<ElasticsearchConfig> option)
        {
            _config = new ElasticsearchConfig(option.Value.ConnectionString);
            _entityPendingChanges = new ConcurrentQueue<EntityContextInfo<Entity>>();
            _domainEvents = new ConcurrentQueue<IDomainEvent>();
            _commandDispatcher = new ElsticDbCommandDispatcher();
        }

        public void Configure()
        {
            if (_elasticClient != null)
                return;

            var uris = _config.ConnectionString.Split(';').Select(x => new Uri(x));
            var pool = new StaticConnectionPool(uris, randomize: false)
            {
                SniffedOnStartup = true
            };
            _connectionSettings = new ConnectionSettings(pool);
            _connectionSettings.DefaultMappingFor<Entity>(c => c.Ignore(i => i.DomainEvents));
            _elasticClient = new ElasticClient(_connectionSettings);

            OnModelCreating();
        }

        protected virtual void OnModelCreating()
        {
        }

        #region Commands
        public async Task AddDocumentAsync<T>(T model) where T : Entity
        {
            await semaphoreSlim.WaitAsync();
            if (model.DomainEvents != null && model.DomainEvents.Count != 0)
                foreach (var domainEvent in model.DomainEvents)
                {
                    _domainEvents.Enqueue(domainEvent);
                }

            var data = new EntityContextInfo<Entity>()
            {
                EntityType = model.GetType(),
                Document = model,
                CommandType = CommandType.Add,
                CommandProvider = new InsertProvider<Entity>(_config)
            };
            _entityPendingChanges.Enqueue(data);

            semaphoreSlim.Release();
        }

        public async Task UpdateDocumentAsync<T>(T model) where T : Entity
        {
            await semaphoreSlim.WaitAsync();
            if (model.DomainEvents != null && model.DomainEvents.Count != 0)
                foreach (var domainEvent in model.DomainEvents)
                {
                    _domainEvents.Enqueue(domainEvent);
                }

            var data = new EntityContextInfo<Entity>()
            {
                EntityType = model.GetType(),
                Document = model,
                CommandType = CommandType.Update,
                CommandProvider = new UpdateProvider<Entity>(_config)
            };
            _entityPendingChanges.Enqueue(data);

            semaphoreSlim.Release();
        }

        public async Task DeleteDocumentAsync<T>(T model) where T : Entity
        {
            await semaphoreSlim.WaitAsync();
            if (model.DomainEvents != null && model.DomainEvents.Count != 0)
                foreach (var domainEvent in model.DomainEvents)
                {
                    _domainEvents.Enqueue(domainEvent);
                }

            var data = new EntityContextInfo<Entity>()
            {
                EntityType = typeof(T),
                Document = model,
                CommandType = CommandType.Delete,
                CommandProvider = new DeleteProvider<Entity>(_config)
            };
            _entityPendingChanges.Enqueue(data);

            semaphoreSlim.Release();
        }

        #endregion

        #region SaveChanges
        public async Task SaveChangesAsync(CancellationToken token = default)
        {
            await semaphoreSlim.WaitAsync();

            Configure();

            if (_entityPendingChanges.Count == 0)
                throw new ElasticsearchClientException("Could not found entity to update");

            while (_entityPendingChanges.TryDequeue(out var contextInfo))
            {
                await _commandDispatcher.DispatchAsync(contextInfo, token);
            }

            semaphoreSlim.Release();
        }
        #endregion

        public IReadOnlyCollection<IDomainEvent> GetDomainEvents()
        {
            var list = new List<IDomainEvent>();
            while (_domainEvents.TryDequeue(out var domainEvent))
            {
                list.Add(domainEvent);
            }

            return list;
        }

        public IElasticClient GetClient()
        {
            Configure();

            return _elasticClient;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
