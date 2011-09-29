using System;

namespace Entile.Server.Commands
{
    public class SendTileNotificationCommand : CommandBase
    {
        public Guid UniqueId { get; private set; }
        public Guid NotificationId { get; private set; }
        public string ParamUri { get; private set; }

        public string Title { get; private set; }
        public int Counter { get; private set; }
        public string BackgroundUri { get; private set; }

        public string BackTitle { get; private set; }
        public string BackContent { get; private set; }
        public string BackBackgroundUri { get; private set; }
        
        public int NumberOfAttempts { get; private set; }

        public SendTileNotificationCommand(Guid uniqueId, Guid notificationId, string paramUri, string title, int counter, string backgroundUri, string backTitle, string backContent, string backBackgroundUri, int numberOfAttempts)
        {
            UniqueId = uniqueId;
            NotificationId = notificationId;
            ParamUri = paramUri;
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