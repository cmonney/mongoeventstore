using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoEventStore.Core.Model
{
    public interface IDomainEvent
    {
        [BsonId]
        ObjectId Id { get; set; }
    }
}   