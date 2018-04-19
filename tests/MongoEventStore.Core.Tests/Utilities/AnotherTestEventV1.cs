using System;
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

        public string Id { get; set; }
    }
}