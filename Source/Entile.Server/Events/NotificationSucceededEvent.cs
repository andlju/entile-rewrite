using System;

namespace Entile.Server.Events
{
    public class NotificationSucceededEvent : EventBase
    {
        public Guid NotificationId { get; private set; }
        public int AttemptsLeft { get; private set; }

        public NotificationSucceededEvent(Guid notificationId, int attemptsLeft)
        {
            NotificationId = notificationId;
            AttemptsLeft = attemptsLeft;
        }
    }

    public class ToastNotificationSucceededEvent : NotificationSucceededEvent
    {
        public string Title { get; private set; }
        public string Body { get; private set; }
        public string ParamUri { get; private set; }

        public ToastNotificationSucceededEvent(Guid notificationId, int attemptsLeft, string title, string body, string paramUri) : base(notificationId, attemptsLeft)
        {
            Title = title;
            Body = body;
            ParamUri = paramUri;
        }
    }

    public class RawNotificationSucceededEvent : NotificationSucceededEvent
    {
        public string Content { get; private set; }

        public RawNotificationSucceededEvent(Guid notificationId, int attemptsLeft, string content) : base(notificationId, attemptsLeft)
        {
            Content = content;
        }
    }

    public class TileNotificationSucceededEvent : NotificationSucceededEvent
    {
        public string Title { get; private set; }
        public string BackgroundUri { get; private set; }
        public int Count { get; private set; }

        public string BackTitle { get; private set; }
        public string BackContent { get; private set; }
        public string BackBackgroundUri { get; private set; }

        public string ParamUri { get; private set; }

        public TileNotificationSucceededEvent(Guid notificationId, int attemptsLeft, string title, string backgroundUri, int count, string backTitle, string backContent, string backBackgroundUri, string paramUri) : base(notificationId, attemptsLeft)
        {
            Title = title;
            BackgroundUri = backgroundUri;
            Count = count;
            BackTitle = backTitle;
            BackContent = backContent;
            BackBackgroundUri = backBackgroundUri;
            ParamUri = paramUri;
        }
    }
}