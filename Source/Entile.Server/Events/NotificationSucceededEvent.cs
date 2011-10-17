using System;

namespace Entile.Server.Events
{
    public class NotificationSucceededEvent : EventBase
    {
        public readonly Guid SubscriptionId;
        public readonly Guid NotificationId;
        public readonly int AttemptsLeft;

        public NotificationSucceededEvent(Guid subscriptionId, Guid notificationId, int attemptsLeft)
        {
            SubscriptionId = subscriptionId;
            NotificationId = notificationId;
            AttemptsLeft = attemptsLeft;
        }
    }

    public class ToastNotificationSucceededEvent : NotificationSucceededEvent
    {
        public readonly string Title;
        public readonly string Body;

        public ToastNotificationSucceededEvent(Guid subscriptionId, Guid notificationId, int attemptsLeft, string title, string body) : base(subscriptionId, notificationId, attemptsLeft)
        {
            Title = title;
            Body = body;
        }
    }

    public class RawNotificationSucceededEvent : NotificationSucceededEvent
    {
        public readonly string Content;

        public RawNotificationSucceededEvent(Guid subscriptionId, Guid notificationId, int attemptsLeft, string content) : base(subscriptionId, notificationId, attemptsLeft)
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

        public TileNotificationSucceededEvent(Guid subscriptionId, Guid notificationId, int attemptsLeft, string title, string backgroundUri, int count, string backTitle, string backContent, string backBackgroundUri) : base(subscriptionId, notificationId, attemptsLeft)
        {
            Title = title;
            BackgroundUri = backgroundUri;
            Count = count;
            BackTitle = backTitle;
            BackContent = backContent;
            BackBackgroundUri = backBackgroundUri;
        }
    }
}