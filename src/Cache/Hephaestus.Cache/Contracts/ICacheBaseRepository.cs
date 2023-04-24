using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hephaestus.Cache.Contracts
{
    public interface ICacheBaseRepository<T> where T : class
    {
        Task<T> GetAsync(string key, CancellationToken token = default);
        Task<List<T>> GetListAsync(string key, CancellationToken token = default);
        Task RefreshAsync(string key, CancellationToken token = default);
        Task RemoveAsync(string key, CancellationToken token = default);
        Task SetAsync(string key, T value, CancellationToken token = default);
    }
}
