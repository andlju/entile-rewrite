using System;

namespace Entile.Server.Events
{
    [Serializable]
    public class ClientRegistrationUpdatedEvent : EventBase
    {
        public string NotificationChannel { get; private set; }

        public ClientRegistrationUpdatedEvent(string notificationChannel)
        {
            NotificationChannel = notificationChannel;
        }
    }
}