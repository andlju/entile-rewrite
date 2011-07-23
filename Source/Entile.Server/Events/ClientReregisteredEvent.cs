using System;

namespace Entile.Server.Events
{
    [Serializable]
    public class ClientReregisteredEvent : EventBase
    {
        public string NotificationChannel { get; private set; }

        public ClientReregisteredEvent(string uniqueId, string notificationChannel)
        {
            UniqueId = uniqueId;
            NotificationChannel = notificationChannel;
        }
    }
}