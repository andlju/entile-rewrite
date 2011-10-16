using System;

namespace Entile.Server.Commands
{
    public class UnregisterForNotificationCommand
    {
        public readonly Guid ClientId;
        public readonly Guid NotificationId;

        public UnregisterForNotificationCommand(Guid clientId, Guid notificationId)
        {
            ClientId = clientId;
            NotificationId = notificationId;
        }
    }
}