using System;
using System.Collections.Generic;
using System.Linq;
using Entile.Server.Events;
using Xunit;

namespace Entile.Server.Tests
{
    class TestEvent : IEvent
    {
        public string UniqueId { get; set; }
        public int SequenceNumber { get; set; }
        public long Timestamp { get; set; }

        public string Test { get; set; }
    }

    class TestEventHandler : IMessageHandler<TestEvent>
    {
        private readonly List<TestEvent> _recievedEvents;

        public TestEventHandler()
        {
            _recievedEvents = new List<TestEvent>();
        }


        public List<TestEvent> RecievedEvents
        {
            get { return _recievedEvents; }
        }

        public void Handle(TestEvent command)
        {
            _recievedEvents.Add(command);
        }
    }

    class DummyEventStore : IEventStore
    {
        private readonly List<IEvent> _events;

        public DummyEventStore()
        {
            _events = new List<IEvent>();
        }

        public IEnumerable<IEvent> GetAllEvents(string uniqueId)
        {
            return from e in _events
                   where e.UniqueId == uniqueId
                   select e;
        }

        public void SaveEvents(string uniqueId, IEnumerable<IEvent> events)
        {
            _events.AddRange(events);
        }

        public IEnumerable<IEvent> GetAllEventsSince(long timestamp)
        {
            return from e in _events
                   where e.Timestamp > timestamp
                   select e;
        }
    }

    public class EventStreamConsumerTest
    {

        [Fact]
        public void Running_With_One_New_Event_Returns_One_Event()
        {
            var router = new MessageRouter();
            var handler = new TestEventHandler();
            router.RegisterHandlersIn(handler);

            var bus = new InProcessBus(router);
            var eventStore = new DummyEventStore();
            eventStore.SaveEvents("1337", new[]
                                              {
                                                  new TestEvent() { UniqueId = "1337", SequenceNumber = 1, Test = "Test", Timestamp = new DateTime(2013, 06, 24, 9, 12, 34, 12).Ticks},
                                                  new TestEvent() { UniqueId = "1337", SequenceNumber = 2, Test = "Test 2", Timestamp = new DateTime(2013, 06, 24, 9, 12, 34, 45).Ticks},
                                                  new TestEvent() { UniqueId = "1337", SequenceNumber = 3, Test = "Test 3", Timestamp = new DateTime(2013, 06, 24, 9, 12, 34, 450).Ticks},
                                                  new TestEvent() { UniqueId = "1337", SequenceNumber = 4, Test = "Test 4", Timestamp = new DateTime(2013, 06, 24, 9, 12, 35, 12).Ticks},
                                              });

            var target = new EventStreamDispatcher(bus, eventStore);

            target.Dispatch(new DateTime(2013, 06, 24, 9, 12, 34, 450).Ticks);

            Assert.Equal(1, handler.RecievedEvents.Count);
        }
    }
}