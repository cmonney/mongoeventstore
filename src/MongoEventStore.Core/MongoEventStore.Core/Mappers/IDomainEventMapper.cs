using MongoEventStore.Core.Model;

namespace MongoEventStore.Core.Mappers
{
    public interface IDomainEventMapper
    {
        DomainEvent ConvertToDomainEvent(object objectEvent);
        object ConvertToObjectEvent(DomainEvent domainEvent);
    }
}