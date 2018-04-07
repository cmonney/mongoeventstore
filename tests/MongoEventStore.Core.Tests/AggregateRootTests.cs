using System.Collections.Generic;
using MongoEventStore.Core.Model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MongoEventStore.Core.Tests
{
    [TestFixture]
    public class AggregateRootTests
    {
        [Test]
        public void Aggegrate_Should_Load_Events_From_History()
        {
            var aggregate = new TestAggregateRoot();

            var history = new List<object>
            {
                new TestEventV1(),
                new AnotherTestEventV1()
            };

            aggregate.LoadsFromHistory(history);

            Assert.That(aggregate.TestEventFired, Is.True);
            Assert.That(aggregate.AnotherTestEventFired, Is.True);
        }

        [Test]
        public void Aggregate_Should_ApplyChange_For_NewEvents()
        {
            var aggregate = new TestAggregateRoot();

            aggregate.ApplyChange(new TestEventV1());
            aggregate.ApplyChange(new AnotherTestEventV1());

            Assert.That(aggregate.TestEventFired, Is.True);
            Assert.That(aggregate.AnotherTestEventFired, Is.True);
        }
    }

    public class TestAggregateRoot : AggregateRoot
    {
        public bool TestEventFired { get; set; }

        public bool AnotherTestEventFired { get; set; }

        public void Apply(TestEventV1 @event)
        {
            TestEventFired = true;
        }

        public void Apply(AnotherTestEventV1 @event)
        {
            AnotherTestEventFired = true;
        }
    }

    public class AnotherTestEventV1 : IDomainEvent
    {
        [JsonProperty(PropertyName = "firstname")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "surname")]
        public string SurName { get; set; }

        [JsonProperty(PropertyName = "isvalid")]
        public bool IsValid { get; set; }   
    }

    public class TestEventV1 : IDomainEvent
    {
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "isCaptured")]
        public bool IsCaptured { get; set; }    
    }
}