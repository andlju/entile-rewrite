using System;

namespace Entile.Server.Events
{
    [Serializable]
    public class ClientRegistrationUpdatedEvent : EventBase
    {
        public readonly string NotificationChannel;

        public ClientRegistrationUpdatedEvent(string notificationChannel)
        {
            NotificationChannel = notificationChannel;
        }
    }
}