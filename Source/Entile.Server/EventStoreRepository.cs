using System;
using System.Linq;
using Entile.Server.Domain;

namespace Entile.Server
{
    public class EventStoreRepository<TDomain> : IRepository<TDomain>
        where TDomain : Aggregate<TDomain>, new()
    {
        private readonly IEventStore _eventStore;
        private readonly IBus _eventBus;

        public EventStoreRepository(IEventStore eventStore, IBus eventBus)
        {
            _eventStore = eventStore;
            _eventBus = eventBus;
        }

        public TDomain GetById(Guid uniqueId)
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
            var events = aggregate.GetUncommittedEvents().ToArray();
            aggregate.ClearUncommittedEvents();
            _eventStore.SaveEvents(aggregate.Id, events);
            foreach(var ev in events)
            {
                _eventBus.Publish(ev);
            }
        }
    }
}