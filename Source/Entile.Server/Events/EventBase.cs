using System;

namespace Entile.Server.Events
{
    [Serializable]
    public abstract class EventBase : IEvent
    {
        public string UniqueId { get; set; }
        public int SequenceNumber { get; set; }
        public long Timestamp { get; set; }

        protected EventBase()
        {
            Timestamp = DateTime.Now.Ticks;
        }
    }
}