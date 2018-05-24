using MongoDB.Bson;
using MongoEventStore.Core.Mappers;
using MongoEventStore.Core.Model;
using Newtonsoft.Json;

namespace MongoEventStore.Core.Tests.Utilities
{
    public class TestEventV1 : IDomainEvent
    {
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "isCaptured")]
        public bool IsCaptured { get; set; }

        [JsonProperty(PropertyName = "id")]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
    }
}