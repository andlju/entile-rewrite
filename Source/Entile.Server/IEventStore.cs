using System.Collections.Generic;
using Entile.Server.Events;

namespace Entile.Server
{
    public interface IEventStore
    {
        IEnumerable<IEvent> GetAllEvents(string uniqueId);
        void SaveEvents(string uniqueId, IEnumerable<IEvent> events);
    }
}