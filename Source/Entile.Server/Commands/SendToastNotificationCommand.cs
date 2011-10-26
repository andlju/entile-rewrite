using System;

namespace Entile.Server.Commands
{
    public class SendToastNotificationCommand : CommandBase
    {
        public Guid ClientId;
        public Guid SubscriptionId;
        public Guid NotificationId;
        public string Title;
        public string Body;
        public int NumberOfAttempts;

        public SendToastNotificationCommand()
        {
            
        }

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