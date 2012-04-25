using System;

namespace Entile.Server.Commands
{
    public class SendRawNotificationCommand : CommandBase
    {
        public Guid ClientId { get; set; }
        public Guid SubscriptionId { get; set; }
        public Guid NotificationId { get; set; }
        public string Content { get; set; }
        public int NumberOfAttempts { get; set; }

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