using System.Collections.Generic;
using System.Threading.Tasks;
using MongoEventStore.Core.Model;

namespace MongoEventStore.Core.Repository
{
    public interface IEventStore
    {
        Task SaveDomainEventsAsync(IEnumerable<DomainEvent> events);
        Task<List<DomainEvent>> GetDomainEventsAsync(string aggregateId);
    }
}   