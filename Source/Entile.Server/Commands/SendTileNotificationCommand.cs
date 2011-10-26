using System;

namespace Entile.Server.Commands
{
    public class SendTileNotificationCommand : CommandBase
    {
        public Guid ClientId;
        public Guid SubscriptionId;
        public Guid NotificationId;

        public string Title;
        public int Counter;
        public string BackgroundUri;

        public string BackTitle;
        public string BackContent;
        public string BackBackgroundUri;

        public int NumberOfAttempts;

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