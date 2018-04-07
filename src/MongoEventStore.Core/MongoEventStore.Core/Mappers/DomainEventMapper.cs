using System;
using System.Linq;
using MongoEventStore.Core.Model;
using Newtonsoft.Json;

namespace MongoEventStore.Core.Mappers
{
    public class DomainEventMapper : IDomainEventMapper
    {
        public DomainEvent ConvertToDomainEvent(object objectEvent)
        {
            var typeName = objectEvent.GetType().Name;
            var type = typeName.Substring(0, typeName.LastIndexOf("V", StringComparison.Ordinal));
            var version = int.Parse(typeName.Substring(typeName.LastIndexOf("V", StringComparison.Ordinal) + 1));
            var json = JsonConvert.SerializeObject(objectEvent, Formatting.None, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

            return new DomainEvent
            {
                Type = type,
                Version = version,
                Json = json
            };
        }

        public object ConvertToObjectEvent(DomainEvent domainEvent)
        {
            var eventName = $"{domainEvent.Type}V{domainEvent.Version}";
            var types = TypeHelper.GetLoadedTypes<IDomainEvent>();
            var eventType = types.Single(x => x.Name == eventName);
            var eventBody = Activator.CreateInstance(eventType);
            JsonConvert.PopulateObject(domainEvent.Json, eventBody);
            return eventBody;
        }
    }
}