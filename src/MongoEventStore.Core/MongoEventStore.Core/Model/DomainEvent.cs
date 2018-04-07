using System;

namespace MongoEventStore.Core.Model
{
    public class DomainEvent
    {
        public int Id { get; set; }

        public string AggregateId { get; set; }

        public string Type { get; set; }

        public int Version { get; set; }

        public string Json { get; set; }

        public long Commit { get; set; }

        public long Index { get; set; }

        public DateTime Timestamp { get; set; }
    }
}