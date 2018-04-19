using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoEventStore.Core.Mappers;
using MongoEventStore.Core.Model;
using MongoEventStore.Core.Repository;
using MongoEventStore.Core.Tests.Utilities;
using NUnit.Framework;

namespace MongoEventStore.Core.Tests
{
    [TestFixture]
    public class AggregateRepositoryTests
    {
        private readonly string _aggregateId = Guid.NewGuid().ToString();
        private IEventStore _eventStore;
        private IDomainEventMapper _domainEventMapper;
        private IAggregateRepository<TestAggregateRoot> _aggregateRepository;

        [SetUp]
        public void SetUp()
        {
            _eventStore = new TestEventStore();
            _domainEventMapper = new DomainEventMapper();
            _aggregateRepository = new AggregateRepository<TestAggregateRoot>(_eventStore, _domainEventMapper);
        }

        [TearDown]
        public void TearDown()
        {
            _aggregateRepository = null;
            _eventStore = null;
            _domainEventMapper = null;
        }

        [Test]
        public async Task GetAggregateAsync_should_applychanges_from_events_history()
        {
            await _eventStore.SaveDomainEventsAsync(new List<DomainEvent>()
            {
                new DomainEvent()
                {
                    Id = Guid.NewGuid().ToString(),
                    AggregateId = _aggregateId,
                    Commit = 1,
                    Index = 1,
                    Type = "AnotherTestEvent",
                    Version = 1,
                    Json = @"{""firstname"":""Jones"",""surname"":""Smith"", ""isvalid"": true}"
                },
                new DomainEvent()
                {
                    Id = Guid.NewGuid().ToString(),
                    AggregateId = _aggregateId,
                    Commit = 2,
                    Index = 2,
                    Type = "TestEvent",
                    Version = 1,
                    Json = @"{""city"":""Hemel Hempstead"",""isCaptured"":false}"
                },
                new DomainEvent()
                {
                    Id = Guid.NewGuid().ToString(),
                    AggregateId = "some-fake-id",
                    Commit = 3,
                    Index = 3,
                    Type = "TestEvent",
                    Version = 1,
                    Json = @"{""city"":""London"",""isCaptured"":true}"
                }
            });

            var aggregate = await _aggregateRepository.GetAggregateAsync(_aggregateId);
            Assert.That(aggregate, Is.Not.Null);
            Assert.That(aggregate.Index, Is.EqualTo(2));
            Assert.That(aggregate.Commit, Is.EqualTo(2));
            Assert.That(aggregate.ReappliedEvents.Count, Is.EqualTo(2));
            Assert.That(aggregate.ReappliedEvents[0], Is.TypeOf<AnotherTestEventV1>());
            Assert.That(aggregate.ReappliedEvents[1], Is.TypeOf<TestEventV1>());
        }

        [Test]
        public async Task SaveAggregateAsync_save_new_aggregate()
        {
            var aggregate = new TestAggregateRoot(){Id = _aggregateId};
            aggregate.ApplyChange(new AnotherTestEventV1()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Joe",
                SurName = "Bloggs",
                IsValid = true
            });

            await _aggregateRepository.SaveAggregateAsync(aggregate);

            var events = (await _eventStore.GetDomainEventsAsync(_aggregateId)).ToList();
            Assert.That(events.Count, Is.EqualTo(1));
            var lastEvent = events.Last();

            Assert.That(lastEvent.AggregateId, Is.EqualTo(_aggregateId));
            Assert.That(lastEvent.Type, Is.EqualTo("AnotherTestEvent"));
            Assert.That(lastEvent.Version, Is.EqualTo(1));
            Assert.That(lastEvent.Commit, Is.EqualTo(1));
            Assert.That(lastEvent.Index, Is.EqualTo(1));
        }

        [Test]
        public async Task SaveAggregateAsync_saves_all_uncommitted_events()
        {
            var id = Guid.NewGuid().ToString();

            await _eventStore.SaveDomainEventsAsync(new List<DomainEvent>()
            {
                new DomainEvent()
                {
                    Id = id,
                    AggregateId = _aggregateId,
                    Commit = 1,
                    Index = 1,
                    Type = "TestEvent",
                    Version = 1,
                    Json = $@"{{'city': 'Hemel Hempstead', 'isCaptured': false, 'Id': '{id}'}}"
                }
            });

            var aggregate = await _aggregateRepository.GetAggregateAsync(_aggregateId);
            aggregate.ApplyChange(new AnotherTestEventV1()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Joe",
                SurName = "Bloggs",
                IsValid = true
            });

            await _aggregateRepository.SaveAggregateAsync(aggregate);

            var events = (await _eventStore.GetDomainEventsAsync(_aggregateId)).ToList();
            Assert.That(events.Count, Is.EqualTo(2));
            var lastEvent = events.Last();

            Assert.That(lastEvent.AggregateId, Is.EqualTo(_aggregateId));
            Assert.That(lastEvent.Type, Is.EqualTo("AnotherTestEvent"));
            Assert.That(lastEvent.Version, Is.EqualTo(1));
            Assert.That(lastEvent.Commit, Is.EqualTo(2));
            Assert.That(lastEvent.Index, Is.EqualTo(3));
        }
    }
}