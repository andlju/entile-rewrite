using System;

namespace Entile.Server.Commands
{
    public class SendRawNotificationCommand : CommandBase
    {
        public readonly Guid ClientId;
        public readonly Guid SubscriptionId;
        public readonly Guid NotificationId;
        public readonly string Content;
        public readonly int NumberOfAttempts;

        public SendRawNotificationCommand(Guid clientId, Guid subscriptionId, Guid notificationId, string content, int numberOfAttempts)
        {
            ClientId = clientId;
            SubscriptionId = subscriptionId;
            NotificationId = notificationId;
            Content = content;
            NumberOfAttempts = numberOfAttempts;
        }
    }
}