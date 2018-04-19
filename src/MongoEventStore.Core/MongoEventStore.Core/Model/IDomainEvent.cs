namespace MongoEventStore.Core.Model
{
    public interface IDomainEvent
    {
        string Id { get; set; }
    }
}