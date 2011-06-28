using System;
using System.Linq;
using Entile.Server.Domain;

namespace Entile.Server
{
    public class EventStoreRepository<TDomain> : IRepository<TDomain>
        where TDomain : Aggregate<TDomain>, new()
    {
        private readonly IEventStore _eventStore;

        public EventStoreRepository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public TDomain GetById(string uniqueId)
        {
            var events = _eventStore.GetAllEvents(uniqueId);
            if (events == null)
                return null;

            var aggregate = new TDomain();

            aggregate.LoadEvents(events);

            return aggregate;
        }

        public void SaveChanges(TDomain aggregate)
        {
            var events = aggregate.GetUncommittedEvents();
            _eventStore.SaveEvents(aggregate.UniqueId, events);
        }
    }
}