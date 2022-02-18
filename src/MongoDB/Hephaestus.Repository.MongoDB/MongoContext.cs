using Hephaestus.Repository.Abstraction.Base;
using Hephaestus.Repository.Abstraction.Contract;
using Hephaestus.Repository.MongoDB.Configure;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hephaestus.Repository.MongoDB
{
    public class MongoContext : IMongoContext
    {
        //Database should be private, after test 
        public IMongoDatabase Database { get; set; }
        public IClientSessionHandle Session { get; set; }
        public MongoClient MongoClient { get; set; }
        private readonly ConcurrentQueue<Func<Task>> _commands;
        private readonly ConcurrentQueue<IDomainEvent> _domainEvents;
        private readonly MongoDbConfig _config;
        public MongoContext(IOptions<MongoDbConfig> option)
        {
            _config = option.Value;
            _commands = new ConcurrentQueue<Func<Task>>();
        }

        private void Configure()
        {
            if (MongoClient != null)
                return;

            MongoClient = new MongoClient(_config.ConnectionString);
            Database = MongoClient.GetDatabase(_config.DatabaseName);
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

        public void AddCommand<T, TKey>(T model, Func<Task> func, CancellationToken token) where T : Entity<TKey>, new()
        {
            if (model.DomainEvents != null && model.DomainEvents.Count != 0)
                foreach (var domainEvent in model.DomainEvents)
                {
                    _domainEvents.Enqueue(domainEvent);
                }

            _commands.Enqueue(() =>
                AddAsync<T>(model, func, token));
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            Configure();

            return Database.GetCollection<T>(name);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            Configure();

            using (Session = await MongoClient.StartSessionAsync(cancellationToken: cancellationToken))
            {
                Session.StartTransaction();

                var commandTasks = _commands.Select(c => c());

                await Task.WhenAll(commandTasks);

                await Session.CommitTransactionAsync();
            }

            return _commands.Count;
        }

        public void Dispose()
        {
            Session?.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
