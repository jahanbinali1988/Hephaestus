using Hephaestus.Repository.Abstraction.Contract;
using Hephaestus.Repository.MongoDB.Configure;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hephaestus.Repository.MongoDB
{
    public class MongoContext : IDisposable
    {
        protected IMongoDatabase Database { get; set; }
        protected MongoClient MongoClient { get; set; }
        private IClientSessionHandle Session { get; set; }
        private ConcurrentQueue<Func<Task>> _commands;
        private ConcurrentQueue<IDomainEvent> _domainEvents;
        private readonly MongoDbCommandDispatcher _commandDispatcher;
        private readonly MongoDbConfig _config;
        public MongoContext(IOptions<MongoDbConfig> option)
        {
            _config = new MongoDbConfig(option.Value.ConnectionString, option.Value.DatabaseName);
            _commands = new ConcurrentQueue<Func<Task>>();
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
            Configure();

            BsonDefaults.GuidRepresentation = GuidRepresentation.CSharpLegacy;
            //BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.CSharpLegacy));

            // Conventions
            var pack = new ConventionPack
                {
                    new IgnoreExtraElementsConvention(true),
                    new IgnoreIfDefaultConvention(true)
                };
            ConventionRegistry.Register("My Solution Conventions", pack, t => true);
        }


        #region Commands
        public void AddCommand(Func<Task> func)
        {
            _commands.Enqueue(func);
        }

        #endregion

        #region SaveChanges
        public async Task SaveChangesAsync(CancellationToken token = default)
        {
            Configure();

            using (Session = await MongoClient.StartSessionAsync())
            {
                Session.StartTransaction();

                var commandTasks = _commands.Select(c => c());

                await Task.WhenAll(commandTasks);

                await Session.CommitTransactionAsync();
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
            Session?.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
