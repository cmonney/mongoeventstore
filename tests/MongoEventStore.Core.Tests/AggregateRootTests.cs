using System.Collections.Generic;
using MongoEventStore.Core.Tests.Utilities;
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
}