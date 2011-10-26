using System;

namespace Entile.Server.Events
{
    [Serializable]
    public abstract class EventBase : IEvent
    {
        public Guid AggregateId { get; set; }

        protected EventBase()
        {

        }
    }
}