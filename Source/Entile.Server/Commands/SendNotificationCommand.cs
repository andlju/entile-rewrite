using System;

namespace Entile.Server.Commands
{
    public class SendToastNotificationCommand : CommandBase
    {
        public string UniqueId { get; private set; }
        public Guid NotificationId { get; private set; }
        public string Title { get; private set; }
        public string Body { get; private set; }
        public string ParamUri { get; private set; }
        public int NumberOfAttempts { get; private set; }

        public SendToastNotificationCommand(string uniqueId, Guid notificationId, string title, string body, string paramUri, int numberOfAttempts)
        {
            UniqueId = uniqueId;
            NotificationId = notificationId;
            Title = title;
            Body = body;
            ParamUri = paramUri;
            NumberOfAttempts = numberOfAttempts;
        }
    }

    public class SendRawTileNotificationCommand : CommandBase
    {
        public Guid NotificationId { get; private set; }
    }

    public class SendTileNotificationCommand : CommandBase
    {
        public string UniqueId { get; private set; }
        public Guid NotificationId { get; private set; }
        public string ParamUri { get; private set; }

        public string Title { get; private set; }
        public int Count { get; private set; }
        public string BackgroundUri { get; private set; }

        public string BackTitle { get; private set; }
        public string BackContent { get; private set; }
        public string BackBackgroundUri { get; private set; }
        
        public int NumberOfAttempts { get; private set; }

        public SendTileNotificationCommand(string uniqueId, Guid notificationId, string paramUri, string title, int count, string backgroundUri, string backTitle, string backContent, string backBackgroundUri, int numberOfAttempts)
        {
            UniqueId = uniqueId;
            NotificationId = notificationId;
            ParamUri = paramUri;
            Title = title;
            Count = count;
            BackgroundUri = backgroundUri;
            BackTitle = backTitle;
            BackContent = backContent;
            BackBackgroundUri = backBackgroundUri;
            NumberOfAttempts = numberOfAttempts;
        }
    }
}