using System.Collections.Generic;
using MongoEventStore.Core.Model;

namespace MongoEventStore.Core.Tests.Utilities
{
    public class TestAggregateRoot : AggregateRoot
    {
        public bool TestEventFired { get; set; }

        public bool AnotherTestEventFired { get; set; }
        public List<dynamic> ReappliedEvents { get; } = new List<dynamic>();

        public void Apply(TestEventV1 @event)
        {
            TestEventFired = true;
            ReappliedEvents.Add(@event);
        }

        public void Apply(AnotherTestEventV1 @event)
        {
            AnotherTestEventFired = true;
            ReappliedEvents.Add(@event);
        }
    }
}