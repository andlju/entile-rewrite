using System;

namespace Entile.Server.Events
{
    public class NotificationSucceededEvent : EventBase
    {
        public readonly Guid NotificationId;
        public readonly int AttemptsLeft;

        public NotificationSucceededEvent(Guid notificationId, int attemptsLeft)
        {
            NotificationId = notificationId;
            AttemptsLeft = attemptsLeft;
        }
    }

    public class ToastNotificationSucceededEvent : NotificationSucceededEvent
    {
        public readonly string Title;
        public readonly string Body;
        public readonly string ParamUri;

        public ToastNotificationSucceededEvent(Guid notificationId, int attemptsLeft, string title, string body, string paramUri) : base(notificationId, attemptsLeft)
        {
            Title = title;
            Body = body;
            ParamUri = paramUri;
        }
    }

    public class RawNotificationSucceededEvent : NotificationSucceededEvent
    {
        public readonly string Content;

        public RawNotificationSucceededEvent(Guid notificationId, int attemptsLeft, string content) : base(notificationId, attemptsLeft)
        {
            Content = content;
        }
    }

    public class TileNotificationSucceededEvent : NotificationSucceededEvent
    {
        public readonly string Title;
        public readonly string BackgroundUri;
        public readonly int Count;

        public readonly string BackTitle;
        public readonly string BackContent;
        public readonly string BackBackgroundUri;

        public readonly string ParamUri;

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