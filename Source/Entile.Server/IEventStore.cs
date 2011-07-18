using System;
using System.Collections.Generic;
using Entile.Server.Events;

namespace Entile.Server
{
    public interface IEventStore
    {
        IEnumerable<IEvent> GetAllEvents(string uniqueId);
        void SaveEvents(string uniqueId, IEnumerable<IEvent> events);
        IEnumerable<IEvent> GetAllEventsSince(long timestamp);
    }

    public interface IEventConsumer
    {
        long GetLastUpdate(string consumerName);
        void SetLastUpdate(string consumerName, long lastUpdate);
    }
}