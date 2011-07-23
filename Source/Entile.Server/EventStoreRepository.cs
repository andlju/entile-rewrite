using System;
using System.Linq;
using Entile.Server.Domain;

namespace Entile.Server
{
    public class EventStoreRepository<TDomain> : IRepository<TDomain>
        where TDomain : Aggregate<TDomain>
    {
        private readonly IEventStore _eventStore;
        private readonly IBus _eventBus;
        private readonly Func<TDomain> _factoryMethod;

        public EventStoreRepository(IEventStore eventStore, IBus eventBus, Func<TDomain> factoryMethod)
        {
            _eventStore = eventStore;
            _eventBus = eventBus;
            _factoryMethod = factoryMethod;
        }

        public TDomain GetById(string uniqueId)
        {
            var events = _eventStore.GetAllEvents(uniqueId);
            if (events == null)
                return null;

            var aggregate = _factoryMethod();

            aggregate.LoadEvents(events);

            return aggregate;
        }

        public void SaveChanges(TDomain aggregate)
        {
            var events = aggregate.GetUncommittedEvents().ToArray();
            aggregate.ClearUncommittedEvents();
            _eventStore.SaveEvents(aggregate.UniqueId, events);
            foreach(var ev in events)
            {
                _eventBus.Publish(ev);
            }
        }
    }
}