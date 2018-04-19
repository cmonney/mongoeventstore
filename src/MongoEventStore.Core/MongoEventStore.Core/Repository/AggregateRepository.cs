using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoEventStore.Core.Mappers;
using MongoEventStore.Core.Model;

namespace MongoEventStore.Core.Repository
{
    public class AggregateRepository<TAggregate> : IAggregateRepository<TAggregate> where TAggregate : AggregateRoot, new()
    {
        private readonly IEventStore _eventStore;
        private readonly IDomainEventMapper _eventMapper;

        public AggregateRepository(IEventStore eventStore, IDomainEventMapper eventMapper)
        {
            _eventStore = eventStore;
            _eventMapper = eventMapper;
        }

        public async Task<TAggregate> GetAggregateAsync(string aggregateId)
        {
            var domainEvents = await _eventStore.GetDomainEventsAsync(aggregateId);
            var events = domainEvents.OrderBy(a => a.Index).ToList();

            if (!events.Any())
            {
                return null;
            }

            var aggregate = new TAggregate
            {
                Id = aggregateId
            };

            foreach (var @event in events)
            {
                var eventBody = _eventMapper.ConvertToObjectEvent(@event);
                aggregate.ApplyChange(eventBody);
            }

            aggregate.Commit = events.Max(x => x.Commit);
            aggregate.Index = events.Max(x => x.Index);

            return aggregate;
        }

        public async Task SaveAggregateAsync(TAggregate aggregate)
        {
            aggregate.Commit += 1;
            var eventRecords = new List<DomainEvent>();

            foreach (var uncommittedEvent in aggregate.GetUncommittedChanges())
            {
                aggregate.Index += 1;

                var @event = _eventMapper.ConvertToDomainEvent(uncommittedEvent);
                @event.AggregateId = aggregate.Id;
                @event.Commit = aggregate.Commit;
                @event.Index = aggregate.Index;
                

                eventRecords.Add(@event);
            }

            await _eventStore.SaveDomainEventsAsync(eventRecords.ToArray());
            aggregate.MarkChangesAsCommitted();
        }
    }
}