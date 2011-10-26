using System;

namespace Entile.Server.Events
{
    public interface IEvent : IMessage
    {
        Guid AggregateId { get; set; }
    }
}