using System;

namespace Entile.Server.Events
{
    public class NotificationFailedEvent : EventBase
    {
        public readonly Guid SubscriptionId;
        public readonly Guid NotificationId;
        public readonly int AttemptsLeft;
        public readonly DateTime LastAttempt;
        public readonly int HttpStatusCode;
        public readonly string NotificationStatus;
        public readonly string DeviceConnectionStatus;
        public readonly string SubscriptionStatus;

        public NotificationFailedEvent(Guid subscriptionId, Guid notificationId, int attemptsLeft, DateTime lastAttempt, int httpStatusCode, string notificationStatus, string deviceConnectionStatus, string subscriptionStatus)
        {
            SubscriptionId = subscriptionId;
            NotificationId = notificationId;
            AttemptsLeft = attemptsLeft;
            LastAttempt = lastAttempt;
            HttpStatusCode = httpStatusCode;
            NotificationStatus = notificationStatus;
            DeviceConnectionStatus = deviceConnectionStatus;
            SubscriptionStatus = subscriptionStatus;
        }
    }

    public class ToastNotificationFailedEvent : NotificationFailedEvent
    {
        public readonly string Title;
        public readonly string Body;

        public ToastNotificationFailedEvent(Guid subscriptionId, Guid notificationId, int attemptsLeft, DateTime lastAttempt, int httpStatusCode, string notificationStatus, string deviceConnectionStatus, string subscriptionStatus, string title, string body) : base(subscriptionId, notificationId, attemptsLeft, lastAttempt, httpStatusCode, notificationStatus, deviceConnectionStatus, subscriptionStatus)
        {
            Title = title;
            Body = body;
        }
    }

    public class RawNotificationFailedEvent : NotificationFailedEvent
    {
        public readonly string Content;

        public RawNotificationFailedEvent(Guid subscriptionId, Guid notificationId, int attemptsLeft, DateTime lastAttempt, int httpStatusCode, string notificationStatus, string deviceConnectionStatus, string subscriptionStatus, string content) : base(subscriptionId, notificationId, attemptsLeft, lastAttempt, httpStatusCode, notificationStatus, deviceConnectionStatus, subscriptionStatus)
        {
            Content = content;
        }
    }

    public class TileNotificationFailedEvent : NotificationFailedEvent
    {
        public readonly string Title;
        public readonly string BackgroundUri;
        public readonly int Count;

        public readonly string BackTitle;
        public readonly string BackContent;
        public readonly string BackBackgroundUri;

        public TileNotificationFailedEvent(Guid subscriptionId, Guid notificationId, int attemptsLeft, DateTime lastAttempt, int httpStatusCode, string notificationStatus, string deviceConnectionStatus, string subscriptionStatus, string title, string backgroundUri, int count, string backTitle, string backContent, string backBackgroundUri) : base(subscriptionId, notificationId, attemptsLeft, lastAttempt, httpStatusCode, notificationStatus, deviceConnectionStatus, subscriptionStatus)
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