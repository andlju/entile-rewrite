using System;

namespace Entile.Server.Commands
{
    public class SendRawNotificationCommand : CommandBase
    {
        public readonly Guid ClientId;
        public readonly Guid NotificationId;
        public readonly string Content;
        public readonly int NumberOfAttempts;

        public SendRawNotificationCommand(Guid clientId, Guid notificationId, string content, int numberOfAttempts)
        {
            ClientId = clientId;
            NotificationId = notificationId;
            Content = content;
            NumberOfAttempts = numberOfAttempts;
        }
    }
}