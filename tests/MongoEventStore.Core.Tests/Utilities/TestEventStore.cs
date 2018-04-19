using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoEventStore.Core.Model;
using MongoEventStore.Core.Repository;

namespace MongoEventStore.Core.Tests.Utilities
{
    public class TestEventStore : IEventStore
    {
        private readonly Dictionary<string, DomainEvent> _events = new Dictionary<string, DomainEvent>();

        public Task SaveDomainEventsAsync(IEnumerable<DomainEvent> events)
        {
            foreach (var domainEvent in events)
            {
                if (!_events.ContainsKey(domainEvent.Id))
                {
                    _events.Add(domainEvent.Id, domainEvent);
                }
                else
                {
                    _events[domainEvent.Id] = domainEvent;
                }
                
            }

            return Task.FromResult(true);
        }

        public Task<List<DomainEvent>> GetDomainEventsAsync(string aggregateId)
        {
            var result = new List<DomainEvent>();

            foreach (var key in _events.Keys)
            {
                if (_events[key].AggregateId == aggregateId)
                {
                    result.Add(_events[key]);
                }
            }

            return Task.FromResult(result.ToList());
        }
    }
}