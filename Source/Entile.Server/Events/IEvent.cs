using System;

namespace Entile.Server.Events
{
    public interface IEvent : IMessage
    {
        Guid UniqueId { get; set; }
        int SequenceNumber { get; set; }
    }
}