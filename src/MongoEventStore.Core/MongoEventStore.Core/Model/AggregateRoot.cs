using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoEventStore.Core.Model
{
    public abstract class AggregateRoot
    {
        private readonly List<object> _changes = new List<object>();

        [BsonId]
        public ObjectId Id { get; set; }

        public long Commit { get; set; }
        public long Index { get; set; }
        public int Version { get; internal set; }

        public IEnumerable<object> GetUncommittedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public void LoadsFromHistory(IEnumerable<object> history)
        {
            foreach (var e in history) ApplyChange(e, false);
        }

        public void ApplyChange(dynamic @event)
        {
            ApplyChange(@event, true);
        }

        private void ApplyChange(dynamic @event, bool isNew)
        {
            dynamic dynamicThis = this;
            dynamicThis.Apply(@event);
            if (isNew) _changes.Add(@event);
        }
    }
}