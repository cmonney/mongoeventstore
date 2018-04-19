using System;
using System.Threading.Tasks;
using MongoEventStore.Core.Model;

namespace MongoEventStore.Core.Repository
{
    public interface IAggregateRepository<TAggregate> where TAggregate : AggregateRoot
    {
        Task<TAggregate> GetAggregateAsync(string aggregateId);

        Task SaveAggregateAsync(TAggregate aggregate);
    }
}