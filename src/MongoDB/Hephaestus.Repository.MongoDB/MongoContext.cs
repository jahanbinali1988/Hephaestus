using Hephaestus.Repository.Abstraction.Base;
using Hephaestus.Repository.Abstraction.Contract;
using Hephaestus.Repository.MongoDB.Configure;
using Hephaestus.Repository.MongoDB.Provider;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hephaestus.Repository.MongoDB
{
    public abstract class MongoContext : IDisposable
    {
        protected IMongoDatabase Database { get; set; }
        protected MongoClient MongoClient { get; set; }
        private ConcurrentQueue<EntityContextInfo<Entity>> _entityPendingChanges;
        private ConcurrentQueue<IDomainEvent> _domainEvents;
        private readonly MongoDbCommandDispatcher _commandDispatcher;
        private readonly MongoDbConfig _config;
        public MongoContext(IOptions<MongoDbConfig> option)
        {
            _config = new MongoDbConfig(option.Value.ConnectionString, option.Value.DatabaseName);
            _entityPendingChanges = new ConcurrentQueue<EntityContextInfo<Entity>>();
            _domainEvents = new ConcurrentQueue<IDomainEvent>();
            _commandDispatcher = new MongoDbCommandDispatcher();
        }

        public void Configure()
        {
            if (MongoClient != null)
                return;

            var mongoClientSettings = new MongoUrl(_config.ConnectionString);
            MongoClient = new MongoClient(mongoClientSettings);
            Database = MongoClient.GetDatabase(_config.DatabaseName);
        }

        protected virtual void OnModelCreating()
        {
        }

        #region Commands
        public void AddDocument<T>(T model) where T : Entity
        {
            if (model.DomainEvents != null && model.DomainEvents.Count != 0)
                foreach (var domainEvent in model.DomainEvents)
                {
                    _domainEvents.Enqueue(domainEvent);
                }

            var contextInfo = new EntityContextInfo<Entity>()
            {
                EntityType = model.GetType(),
                Document = model,
                CommandType = CommandType.Add,
                CommandProvider = new InsertProvider<Entity>(Database.GetCollection<Entity>(model.GetType().Name))
            };
            _entityPendingChanges.Enqueue(contextInfo);
        }

        public void UpdateDocument<T>(T model) where T : Entity
        {
            if (model.DomainEvents != null && model.DomainEvents.Count != 0)
                foreach (var domainEvent in model.DomainEvents)
                {
                    _domainEvents.Enqueue(domainEvent);
                }

            var contextInfo = new EntityContextInfo<Entity>()
            {
                EntityType = model.GetType(),
                Document = model,
                CommandType = CommandType.Update,
                CommandProvider = new UpdateProvider<Entity>(Database.GetCollection<Entity>(model.GetType().Name))
            };
            _entityPendingChanges.Enqueue(contextInfo);
        }

        public void DeleteDocument<T>(T model) where T : Entity
        {
            if (model.DomainEvents != null && model.DomainEvents.Count != 0)
                foreach (var domainEvent in model.DomainEvents)
                {
                    _domainEvents.Enqueue(domainEvent);
                }

            var contextInfo = new EntityContextInfo<Entity>()
            {
                EntityType = model.GetType(),
                Document = model,
                CommandType = CommandType.Delete,
                CommandProvider = new DeleteProvider<Entity>(Database.GetCollection<Entity>(model.GetType().Name))
            };
            _entityPendingChanges.Enqueue(contextInfo);
        }
        #endregion

        #region SaveChanges
        public async Task SaveChangesAsync(CancellationToken token = default)
        {
            Configure();

            if (_entityPendingChanges.Count == 0)
                throw new MongoConfigurationException("Could not found entity to update");

            while (_entityPendingChanges.TryDequeue(out var contextInfo))
            {
                await _commandDispatcher.DispatchAsync(contextInfo, token);
            }
        }
        #endregion

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            Configure();

            return Database.GetCollection<T>(name);
        }

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
