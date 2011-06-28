using System;

namespace Entile.Server.Events
{
    [Serializable]
    public abstract class EventBase : IEvent
    {
        public string UniqueId { get; set; }
        public int SequenceNumber { get; set; }

        protected EventBase()
        {
        }
    }
}