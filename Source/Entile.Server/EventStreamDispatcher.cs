using System;

namespace Entile.Server
{
    public class EventStreamDispatcher
    {
        private readonly IBus _bus;
        private readonly IEventStore _eventStore;

        public EventStreamDispatcher(IBus bus, IEventStore eventStore)
        {
            _bus = bus;
            _eventStore = eventStore;
        }

        public long Dispatch(long fromTimestamp)
        {
            long timestamp = fromTimestamp; 

            var events = _eventStore.GetAllEventsSince(fromTimestamp);
            
            if (events == null)
                return timestamp;

            foreach (var e in events)
            {
                _bus.Publish(e);
                timestamp = e.Timestamp;
            }
            return timestamp;
        }

        public long Dispatch(Guid uniqueId)
        {
            var events = _eventStore.GetAllEvents(uniqueId);

            long lastTimestamp = 0;
            foreach (var e in events)
            {
                _bus.Publish(e);
                lastTimestamp = e.Timestamp;
            }
            return lastTimestamp;
        }

    }
}