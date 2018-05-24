using System;
using MongoDB.Bson;
using MongoEventStore.Core.Mappers;
using MongoEventStore.Core.Model;
using Newtonsoft.Json;

namespace MongoEventStore.Core.Tests.Utilities
{
    public class AnotherTestEventV1 : IDomainEvent
    {
        [JsonProperty(PropertyName = "firstname")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "surname")]
        public string SurName { get; set; }

        [JsonProperty(PropertyName = "isvalid")]
        public bool IsValid { get; set; }

        [JsonProperty(PropertyName = "id")]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
    }
}