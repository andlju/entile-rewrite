using System;
using System.Collections.Generic;
using Entile.Server.Events;

namespace Entile.Server
{
    public interface IEventStore
    {
        IEnumerable<IEvent> GetAllEvents(Guid uniqueId);
        void SaveEvents(Guid uniqueId, IEnumerable<IEvent> events);
        IEnumerable<IEvent> GetAllEventsSince(long timestamp);
    }

    public interface IEventConsumer
    {
        long GetLastUpdate(string consumerName);
        void SetLastUpdate(string consumerName, long lastUpdate);
    }
}