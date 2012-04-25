using System;

namespace Entile.Server.Commands
{
    public class SendTileNotificationCommand : CommandBase
    {
        public Guid ClientId { get; set; }
        public Guid SubscriptionId { get; set; }
        public Guid NotificationId { get; set; }

        public string Title { get; set; }
        public int Counter { get; set; }
        public string BackgroundUri { get; set; }

        public string BackTitle { get; set; }
        public string BackContent { get; set; }
        public string BackBackgroundUri { get; set; }

        public int NumberOfAttempts { get; set; }

        public SendTileNotificationCommand()
        {
            
        }

        public SendTileNotificationCommand(Guid clientId, Guid notificationId, Guid subscriptionId, string title, int counter, string backgroundUri, string backTitle, string backContent, string backBackgroundUri, int numberOfAttempts)
        {
            ClientId = clientId;
            SubscriptionId = subscriptionId;
            NotificationId = notificationId;
            Title = title;
            Counter = counter;
            BackgroundUri = backgroundUri;
            BackTitle = backTitle;
            BackContent = backContent;
            BackBackgroundUri = backBackgroundUri;
            NumberOfAttempts = numberOfAttempts;
        }
    }
}