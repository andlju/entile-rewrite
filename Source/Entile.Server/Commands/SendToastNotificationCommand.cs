using System;

namespace Entile.Server.Commands
{
    public class SendToastNotificationCommand : CommandBase
    {
        public readonly Guid ClientId;
        public readonly Guid SubscriptionId;
        public readonly Guid NotificationId;
        public readonly string Title;
        public readonly string Body;
        public readonly int NumberOfAttempts;

        public SendToastNotificationCommand(Guid clientId, Guid subscriptionId, Guid notificationId, string title, string body, int numberOfAttempts)
        {
            ClientId = clientId;
            SubscriptionId = subscriptionId;
            NotificationId = notificationId;
            Title = title;
            Body = body;
            NumberOfAttempts = numberOfAttempts;
        }
    }
}