using System;

namespace Entile.Server.Events
{
    public class NotificationFailedEvent : EventBase
    {
        public Guid NotificationId { get; private set; }
        public int AttemptsLeft { get; private set; }
        public DateTime LastAttempt { get; private set; }
        public int HttpStatusCode { get; private set; }
        public string NotificationStatus { get; private set; }
        public string DeviceConnectionStatus { get; private set; }
        public string SubscriptionStatus { get; private set; }

        public NotificationFailedEvent(Guid notificationId, int attemptsLeft, DateTime lastAttempt, int httpStatusCode, string notificationStatus, string deviceConnectionStatus, string subscriptionStatus)
        {
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
        public string Title { get; private set; }
        public string Body { get; private set; }
        public string ParamUri { get; private set; }

        public ToastNotificationFailedEvent(Guid notificationId, int attemptsLeft, DateTime lastAttempt, int httpStatusCode, string notificationStatus, string deviceConnectionStatus, string subscriptionStatus, string title, string body, string paramUri) : base(notificationId, attemptsLeft, lastAttempt, httpStatusCode, notificationStatus, deviceConnectionStatus, subscriptionStatus)
        {
            Title = title;
            Body = body;
            ParamUri = paramUri;
        }
    }

    public class RawNotificationFailedEvent : NotificationFailedEvent
    {
        public string Content { get; private set; }

        public RawNotificationFailedEvent(Guid notificationId, int attemptsLeft, DateTime lastAttempt, int httpStatusCode, string notificationStatus, string deviceConnectionStatus, string subscriptionStatus, string content) : base(notificationId, attemptsLeft, lastAttempt, httpStatusCode, notificationStatus, deviceConnectionStatus, subscriptionStatus)
        {
            Content = content;
        }
    }

    public class TileNotificationFailedEvent : NotificationFailedEvent
    {
        public string Title { get; private set; }
        public string BackgroundUri { get; private set; }
        public int Count { get; private set; }

        public string BackTitle { get; private set; }
        public string BackContent { get; private set; }
        public string BackBackgroundUri { get; private set; }

        public string ParamUri { get; private set; }

        public TileNotificationFailedEvent(Guid notificationId, int attemptsLeft, DateTime lastAttempt, int httpStatusCode, string notificationStatus, string deviceConnectionStatus, string subscriptionStatus, string title, string backgroundUri, int count, string backTitle, string backContent, string backBackgroundUri, string paramUri) : base(notificationId, attemptsLeft, lastAttempt, httpStatusCode, notificationStatus, deviceConnectionStatus, subscriptionStatus)
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