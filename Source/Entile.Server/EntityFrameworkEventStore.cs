using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using Entile.Server.Events;

namespace Entile.Server
{
    public class Event
    {
        [Key]
        public Guid EventId { get; set; }

        public string UniqueId { get; set; }
        public int SequenceNumber { get; set; }
        public long Timestamp { get; set; }

        public string SerializedEvent { get; set; }

    }

    public class Consumer
    {
        [Key]
        public string Name { get; set; }

        public long LastUpdate { get; set; }
    }

    public class EventContext : DbContext
    {
        public EventContext() : base("Entile")
        {
        }

        public DbSet<Event> Events { get; set; }

        public DbSet<Consumer> Consumers { get; set; }
    }

    public class EntityFrameworkEventStore : IEventStore, IEventConsumer
    {
        private readonly IMessageSerializer _serializer;

        public EntityFrameworkEventStore(IMessageSerializer serializer)
        {
            _serializer = serializer;
        }

        public IEnumerable<IEvent> GetAllEvents(string uniqueId)
        {
            string[] evts;
            using(var context = new EventContext())
            {
                evts = (from e in context.Events
                        where e.UniqueId == uniqueId
                        orderby e.SequenceNumber
                        select e.SerializedEvent).ToArray();

            }
            if (evts.Length == 0)
                return null;
            return evts.Select(ev => _serializer.Deserialize(ev)).Cast<IEvent>();
        }

        public void SaveEvents(string uniqueId, IEnumerable<IEvent> events)
        {
            using(var context = new EventContext())
            {
                foreach(var e in events)
                {
                    context.Events.Add(new Event()
                                           {
                                               EventId = Guid.NewGuid(),
                                               UniqueId = uniqueId, 
                                               SequenceNumber = e.SequenceNumber, 
                                               Timestamp = e.Timestamp,
                                               SerializedEvent = _serializer.Serialize(e)
                                           });
                }
                context.SaveChanges();
            }
        }

        public IEnumerable<IEvent> GetAllEventsSince(long timestamp)
        {
            string[] evts;
            var ticks = timestamp;
            using (var context = new EventContext())
            {
                evts = (from e in context.Events
                        where e.Timestamp > ticks
                        orderby e.Timestamp
                        select e.SerializedEvent).ToArray();

            }
            if (evts.Length == 0)
                return null;

            return evts.Select(ev => _serializer.Deserialize(ev)).Cast<IEvent>();
        }

        public long GetLastUpdate(string consumerName)
        {
            using (var context = new EventContext())
            {
                return (from c in context.Consumers
                        where c.Name == consumerName
                        select c.LastUpdate).FirstOrDefault();
            }
        }

        public void SetLastUpdate(string consumerName, long lastUpdate)
        {
            using (var context = new EventContext())
            {
                var consumer = (from c in context.Consumers where c.Name == consumerName select c).FirstOrDefault();
                if (consumer == null)
                {
                    consumer = new Consumer() { Name = consumerName};
                    context.Consumers.Add(consumer);
                }

                consumer.LastUpdate = lastUpdate;
                context.SaveChanges();
            }
        }
    }
}