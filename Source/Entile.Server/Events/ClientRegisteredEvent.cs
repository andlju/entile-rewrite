using System;

namespace Entile.Server.Events
{
    [Serializable]
    public class ClientRegisteredEvent : EventBase
    {
        public readonly string NotificationChannel;

        public ClientRegisteredEvent(Guid aggregateId, string notificationChannel)
        {
            AggregateId = aggregateId;
            NotificationChannel = notificationChannel;
        }
    }
}