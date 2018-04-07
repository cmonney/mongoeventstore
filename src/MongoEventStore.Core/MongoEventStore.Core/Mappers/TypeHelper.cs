using System;
using System.Collections.Generic;
using System.Linq;

namespace MongoEventStore.Core.Mappers
{
    public static class TypeHelper
    {
        public static IEnumerable<Type> GetLoadedTypes<T>()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);
        }
    }
}