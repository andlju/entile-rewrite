using System;

namespace Entile.Server.Events
{
    [Serializable]
    public class ClientRegisteredEvent : EventBase
    {
        public string NotificationChannel { get; private set; }

        public ClientRegisteredEvent(Guid uniqueId, string notificationChannel)
        {
            UniqueId = uniqueId;
            NotificationChannel = notificationChannel;
        }
    }
}