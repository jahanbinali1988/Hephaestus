using System.Threading.Tasks;

namespace Hephaestus.Repository.Abstraction.EventProcessing.DomainEvent
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync();
    }
}
