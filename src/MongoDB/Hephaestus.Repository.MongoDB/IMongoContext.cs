using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hephaestus.Repository.MongoDB
{
    public interface IMongoContext : IDisposable
    {
        //void AddCommand(Func<Task> func);
        //Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        //IMongoCollection<T> GetCollection<T>(string name);
    }
}
