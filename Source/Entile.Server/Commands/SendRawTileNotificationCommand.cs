using System;

namespace Entile.Server.Commands
{
    public class SendRawNotificationCommand : CommandBase
    {
        public Guid ClientId;
        public Guid SubscriptionId;
        public Guid NotificationId;
        public string Content;
        public int NumberOfAttempts;

        public SendRawNotificationCommand()
        {
            
        }

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