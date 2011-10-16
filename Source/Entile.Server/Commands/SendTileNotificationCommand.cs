using System;

namespace Entile.Server.Commands
{
    public class SendTileNotificationCommand : CommandBase
    {
        public readonly Guid ClientId;
        public readonly Guid NotificationId;
        public readonly string ParamUri;

        public readonly string Title;
        public readonly int Counter;
        public readonly string BackgroundUri;

        public readonly string BackTitle;
        public readonly string BackContent;
        public readonly string BackBackgroundUri;

        public readonly int NumberOfAttempts;

        public SendTileNotificationCommand(Guid clientId, Guid notificationId, string paramUri, string title, int counter, string backgroundUri, string backTitle, string backContent, string backBackgroundUri, int numberOfAttempts)
        {
            ClientId = clientId;
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