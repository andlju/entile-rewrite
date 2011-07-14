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
        [Column(Order = 0)]
        public string UniqueId { get; set; }

        [Key]
        [Column(Order = 1)]
        public int SequenceNumber { get; set; }

        public string SerializedEvent { get; set; }
    }

    public class EventContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
    }

    public class EntityFrameworkEventStore : IEventStore
    {
        private readonly IEventSerializer _serializer;

        public EntityFrameworkEventStore(IEventSerializer serializer)
        {
            _serializer = serializer;
        }

        public IEnumerable<IEvent> GetAllEvents(string uniqueId)
        {
            using(var context = new EventContext())
            {
                var evts = (from e in context.Events
                         where e.UniqueId == uniqueId
                         orderby e.SequenceNumber
                         select e.SerializedEvent).ToArray();

                if (evts.Length == 0)
                    return null;
                return evts.Select(ev => _serializer.Deserialize(ev)).Cast<IEvent>();
            }
        }

        public void SaveEvents(string uniqueId, IEnumerable<IEvent> events)
        {
            using(var context = new EventContext())
            {
                foreach(var e in events)
                {
                    context.Events.Add(new Event()
                                           {
                                               UniqueId = uniqueId, 
                                               SequenceNumber = e.SequenceNumber, 
                                               SerializedEvent = _serializer.Serialize(e)
                                           });
                }
                context.SaveChanges();
            }
        }
    }
}