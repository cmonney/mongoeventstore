using System;
using MongoEventStore.Core.Mappers;
using MongoEventStore.Core.Model;
using MongoEventStore.Core.Tests.Utilities;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MongoEventStore.Core.Tests
{
    [TestFixture]
    public class DomainEventMapperTests
    {
        private IDomainEventMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new DomainEventMapper();
        }

        [Test]
        public void Mapper_Should_ConvertToDomainEvent()
        {
            var id = Guid.NewGuid().ToString();
            var testEvent = new TestEventV1(){City = "Hemel Hempstead", IsCaptured = false, Id = id};

            var result = _mapper.ConvertToDomainEvent(testEvent);

            var expectedJson = JsonConvert.SerializeObject(testEvent);

            Assert.That(result.Type, Is.EqualTo("TestEvent"));
            Assert.That(result.Version, Is.EqualTo(1));
            Assert.That(result.Json, Is.EqualTo(expectedJson));
        }

        [Test]
        public void Mapper_Should_ConvertToObjectEvent()
        {
            var domainEvent = new DomainEvent
            {
                Type = "AnotherTestEvent",
                Version = 1,
                Json = @"{""firstname"":""Jones"",""surname"":""Smith"", ""isvalid"": true}"
            };

            var result = (AnotherTestEventV1) _mapper.ConvertToObjectEvent(domainEvent);

            Assert.That(result.FirstName, Is.EqualTo("Jones"));
            Assert.That(result.SurName, Is.EqualTo("Smith"));
            Assert.That(result.IsValid, Is.EqualTo(true));
        }
    }
}