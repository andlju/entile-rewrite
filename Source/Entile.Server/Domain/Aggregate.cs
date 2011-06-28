using System;
using System.Collections.Generic;
using System.Globalization;
using Entile.Server.Events;

namespace Entile.Server.Domain
{
    public abstract class Aggregate<TDomain>
    {
        protected IDictionary<Type, Action<IEvent>> RegisteredEvents = new Dictionary<Type, Action<IEvent>>();

        protected void RegisterEvent<TEvent>(Action<TEvent> eventHandler)
        {
            RegisteredEvents[typeof(TEvent)] = (h) => eventHandler((TEvent)h);
        }

        private readonly List<IEvent> _eventList = new List<IEvent>();

        protected void ApplyEvent(IEvent @event)
        {
            @event.UniqueId = @event.UniqueId ?? UniqueId;
            @event.SequenceNumber = ++SequenceNumber;

            _eventList.Add(@event);

            HandleEvent(@event);
        }

        public void LoadEvents(IEnumerable<IEvent> events)
        {
            foreach(var @event in events)
            {
                HandleEvent(@event);
                SequenceNumber = @event.SequenceNumber;
            }
        }

        private void HandleEvent(IEvent @event)
        {
            Action<IEvent> eventHandler;
            if (RegisteredEvents.TryGetValue(@event.GetType(), out eventHandler))
                eventHandler(@event);
        }

        public IEnumerable<IEvent> GetUncommittedEvents()
        {
            return _eventList;
        }

        public abstract string UniqueId { get; }
        protected int SequenceNumber { get; set; }
    }
}