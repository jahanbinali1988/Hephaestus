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

namespace Hephaestus.Repository.Elasticsearch
{
    public class ElasticDbContext : IDisposable
    {
        protected IElasticClient _elasticClient;
        protected ConnectionSettings _connectionSettings;
        private ConcurrentQueue<EntityContextInfo> _entityPendingChanges;
        private ConcurrentQueue<IDomainEvent> _domainEvents;
        private readonly ElsticDbCommandDispatcher _commandDispatcher;
        private readonly ElasticsearchConfig _config;
        public ElasticDbContext(IOptions<ElasticsearchConfig> option)
        {
            _config = new ElasticsearchConfig(option.Value.ConnectionString);
            _entityPendingChanges = new ConcurrentQueue<EntityContextInfo>();
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
                SniffedOnStartup = true,
            };
            _connectionSettings = new ConnectionSettings(pool);
            _elasticClient = new ElasticClient(_connectionSettings);
            
            OnModelCreating();
        }

        protected virtual void OnModelCreating()
        {

        }

        #region Commands
        public void AddDocument<T, TKey>(T model) where T : Entity<TKey>
        {
            if (model.DomainEvents != null && model.DomainEvents.Count != 0)
                foreach (var domainEvent in model.DomainEvents)
                {
                    _domainEvents.Enqueue(domainEvent);
                }

            var data = new EntityContextInfo { 
                Id = default, 
                EntityType = model.GetType(), 
                Document = model, 
                CommandType = CommandType.Add, 
                CommandProvider = new InsertProvider(_elasticClient)
            };
            _entityPendingChanges.Enqueue(data);
        }

        public void UpdateDocument<T, TKey>(T model, TKey id) where T : Entity<TKey>
        {
            if (model.DomainEvents != null && model.DomainEvents.Count != 0)
                foreach (var domainEvent in model.DomainEvents)
                {
                    _domainEvents.Enqueue(domainEvent);
                }

            var data = new EntityContextInfo 
                { 
                    Id = id, 
                    EntityType = model.GetType(), 
                    Document = model, 
                    CommandType = CommandType.Update,
                    CommandProvider = new UpdateProvider(_elasticClient)
                };
            _entityPendingChanges.Enqueue(data);
        }

        public void DeleteDocument<T, TKey>(T model) where T : Entity<TKey>
        {
            if (model.DomainEvents != null && model.DomainEvents.Count != 0)
                foreach (var domainEvent in model.DomainEvents)
                {
                    _domainEvents.Enqueue(domainEvent);
                }

            var data = new EntityContextInfo 
            { 
                Id = model.Id, 
                EntityType = typeof(T), 
                Document = null, 
                CommandType = CommandType.Delete,
                CommandProvider = new DeleteProvider(_elasticClient)
            };
            _entityPendingChanges.Enqueue(data);
        }

        #endregion

        #region SaveChanges
        public async Task SaveChangesAsync(CancellationToken token = default)
        {
            Configure();

            if (_entityPendingChanges.Count == 0)
                throw new ElasticsearchClientException("Could not found entity to update");

            while (_entityPendingChanges.TryDequeue(out var task))
            {
                await _commandDispatcher.DispatchAsync(task, token);
            }
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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
