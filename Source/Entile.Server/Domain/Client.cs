using System;
using System.Collections.Generic;
using Entile.Server.Commands;
using Entile.Server.Events;

namespace Entile.Server.Domain
{
    class Subscription
    {
        public Guid SubscriptionId { get; private set; }
        public NotificationKind Kind { get; private set; }
        public string ParamUri { get; private set; }

        public Subscription(Guid subscriptionId, NotificationKind kind, string paramUri)
        {
            SubscriptionId = subscriptionId;
            Kind = kind;
            ParamUri = paramUri;
        }
    }

    public class Client : CommonDomain.Core.AggregateBase<IEvent>
    {
        private bool _isActive;
        private string _notificationChannel;

        private readonly IDictionary<Guid, Subscription> _subscriptions;

        public Client()
        {
            _subscriptions = new Dictionary<Guid, Subscription>();
        }

        public Client(Guid uniqueId, string notificationChannel)
            : this()
        {
            PublishEvent(new ClientRegisteredEvent(uniqueId, notificationChannel));
        }

        public void UpdateRegistration(string notificationChannel)
        {
            PublishEvent(new ClientRegistrationUpdatedEvent(notificationChannel));
        }

        public void Unregister()
        {
            if (!_isActive)
                throw new ClientNotRegisteredException(Id);

            PublishEvent(new ClientUnregisteredEvent());
        }

        public void Subscribe(Guid subscriptionId, NotificationKind notificationKind, string notificationUri, IEnumerable<KeyValuePair<string, string>> extendedInformation)
        {
            PublishEvent(new SubscribedEvent(subscriptionId, notificationKind, notificationUri, extendedInformation));
        }

        public void Unsubscribe(Guid subscriptionId)
        {
            if (!_subscriptions.ContainsKey(subscriptionId))
                throw new UnknownSubscriptionException(Id, subscriptionId);

            PublishEvent(new UnsbuscribedEvent(subscriptionId));
        }

        public void SendToastNotification(Guid subscriptionId, Guid notificationId, string title, string body, int attemptsLeft, INotificationSender notificationSender, IMessageScheduler messageScheduler)
        {
            Subscription subscription;
            if (!_subscriptions.TryGetValue(subscriptionId, out subscription))
            {
                throw new UnknownSubscriptionException(Id, subscriptionId);
            }
            if (subscription.Kind != NotificationKind.Toast)
            {
                throw new InvalidOperationException("Trying to send Toast notification to Tile/Raw subscription.");
            }
            SendNotification(subscriptionId, new ToastNotification(notificationId, title, body, subscription.ParamUri), attemptsLeft, notificationSender, messageScheduler);
        }

        public void SendTileNotification(Guid subscriptionId, Guid notificationId, string title, int counter, string backBackgroundUri, string backTitle, string backgroundUri, string backContent, int attemptsLeft, INotificationSender notificationSender, IMessageScheduler messageScheduler)
        {
            Subscription subscription;
            if (!_subscriptions.TryGetValue(subscriptionId, out subscription))
            {
                throw new UnknownSubscriptionException(Id, subscriptionId);
            }
            if (subscription.Kind != NotificationKind.Tile)
            {
                throw new InvalidOperationException("Trying to send Tile notification to Toast/Raw subscription.");
            }

            SendNotification(subscriptionId, new TileNotification(notificationId, title, counter, backBackgroundUri, backTitle, backgroundUri, backContent, subscription.ParamUri), attemptsLeft, notificationSender, messageScheduler);
        }

        public void SendRawNotification(Guid subscriptionId, Guid notificationId, string rawContent, int attemptsLeft, INotificationSender notificationSender, IMessageScheduler messageScheduler)
        {
            Subscription subscription;
            if (!_subscriptions.TryGetValue(subscriptionId, out subscription))
            {
                throw new UnknownSubscriptionException(Id, subscriptionId);
            }
            if (subscription.Kind != NotificationKind.Raw)
            {
                throw new InvalidOperationException("Trying to send Raw notification to Tile/Toast subscription.");
            }
            SendNotification(subscriptionId, new RawNotification(notificationId, rawContent), attemptsLeft, notificationSender, messageScheduler);
        }

        private void SendNotification(Guid subscriptionId, NotificationBase notification, int attemptsLeft, INotificationSender notificationSender, IMessageScheduler messageScheduler)
        {
            var response = notificationSender.SendNotification(_notificationChannel, notification);

            var toastNotification = notification as ToastNotification;
            var rawNotification = notification as RawNotification;
            var tileNotification = notification as TileNotification;

            if (response.HttpStatusCode == 200)
            {
                if (response.NotificationStatus == "Received")
                {
                    if (toastNotification != null)
                        PublishEvent(new ToastNotificationSucceededEvent(subscriptionId, toastNotification.NotificationId, attemptsLeft, toastNotification.Title, toastNotification.Body));
                    else if (rawNotification != null)
                        PublishEvent(new RawNotificationSucceededEvent(subscriptionId, rawNotification.NotificationId, attemptsLeft, rawNotification.RawContent));
                    else if (tileNotification != null)
                        PublishEvent(new TileNotificationSucceededEvent(subscriptionId, tileNotification.NotificationId, attemptsLeft, tileNotification.Title, tileNotification.BackgroundUri, tileNotification.Counter, tileNotification.BackTitle, tileNotification.BackContent, tileNotification.BackBackgroundUri));
                    return;
                }
            }

            // Something went wrong somehow. First, let's publish an event saing so.
            if (toastNotification != null)
                PublishEvent(new ToastNotificationFailedEvent(subscriptionId, toastNotification.NotificationId, attemptsLeft, DateTime.Now, response.HttpStatusCode, response.NotificationStatus, response.DeviceConnectionStatus, response.SubscriptionStatus, toastNotification.Title, toastNotification.Body));
            else if (rawNotification != null)
                PublishEvent(new RawNotificationFailedEvent(subscriptionId, rawNotification.NotificationId, attemptsLeft, DateTime.Now, response.HttpStatusCode, response.NotificationStatus, response.DeviceConnectionStatus, response.SubscriptionStatus, rawNotification.RawContent));
            else if (tileNotification != null)
                PublishEvent(new TileNotificationFailedEvent(subscriptionId, tileNotification.NotificationId, attemptsLeft, DateTime.Now, response.HttpStatusCode, response.NotificationStatus, response.DeviceConnectionStatus, response.SubscriptionStatus, tileNotification.Title, tileNotification.BackgroundUri, tileNotification.Counter, tileNotification.BackTitle, tileNotification.BackContent, tileNotification.BackBackgroundUri));

            // Next let's figure out what to do next
            var resendTime = DateTime.Now;
            var shouldAttemptAgain = false;

            if (response.NotificationStatus == "QueueFull"  || response.HttpStatusCode == 503)
            {
                // Service currently full or unavailable. 
                resendTime = resendTime.AddMinutes(1); // TODO Increase exponentially
                shouldAttemptAgain = true;
            } 
            else if (response.NotificationStatus == "Suppressed")
            {
                shouldAttemptAgain = false;
            } 
            else if (response.NotificationStatus == "Dropped" && response.SubscriptionStatus == "Expired")
            {
                // Subscription Expired. Don't attempt again and unregister this client.
                shouldAttemptAgain = false;
                Unregister();
            } 
            else if (response.NotificationStatus == "Dropped" && response.SubscriptionStatus == "Active")
            {
                // Rate limit reached. Attempt to resend in one hour
                resendTime = resendTime.AddHours(1);
                shouldAttemptAgain = true;
            } 
            else if (response.NotificationStatus == "Dropped" && response.DeviceConnectionStatus == "Inactive")
            {
                resendTime = resendTime.AddHours(1);
                shouldAttemptAgain = true;
            }

            // Should we re-try?
            if (shouldAttemptAgain)
            {
                attemptsLeft--;
                if (toastNotification != null)
                    messageScheduler.ScheduleMessage(new SendToastNotificationCommand(this.Id, subscriptionId, toastNotification.NotificationId, toastNotification.Title, toastNotification.Body, attemptsLeft), resendTime);
                else if (rawNotification != null)
                    messageScheduler.ScheduleMessage(new SendRawNotificationCommand(this.Id, subscriptionId, rawNotification.NotificationId, rawNotification.RawContent, attemptsLeft), resendTime);
                else if (tileNotification != null)
                    messageScheduler.ScheduleMessage(new SendTileNotificationCommand(this.Id, subscriptionId, tileNotification.NotificationId, tileNotification.Title, tileNotification.Counter, tileNotification.BackgroundUri, tileNotification.BackTitle, tileNotification.BackContent, tileNotification.BackBackgroundUri, attemptsLeft), resendTime);
            }
        }

        protected void PublishEvent(IEvent @event)
        {
            if (@event.AggregateId == Guid.Empty)
                @event.AggregateId = Id;

            RaiseEvent(@event);
        }

        protected void Apply(ClientRegisteredEvent @event)
        {
            Id = @event.AggregateId;
            _isActive = true;
            _notificationChannel = @event.NotificationChannel;
        }

        protected void Apply(ClientRegistrationUpdatedEvent @event)
        {
            _isActive = true;
            _notificationChannel = @event.NotificationChannel;
        }

        protected void Apply(ClientUnregisteredEvent @event)
        {
            _isActive = false;
        }

        protected void Apply(UnsbuscribedEvent @event)
        {
            _subscriptions.Remove(@event.SubscriptionId);
        }

        protected void Apply(SubscribedEvent @event)
        {
            _subscriptions[@event.SubscriptionId] = new Subscription(@event.SubscriptionId, @event.Kind, @event.ParamUri);
        }

        protected void Apply(TileNotificationSucceededEvent @event)
        {

        }
        protected void Apply(ToastNotificationSucceededEvent @event)
        {

        }
        protected void Apply(RawNotificationSucceededEvent @event)
        {

        }
        protected void Apply(TileNotificationFailedEvent @event)
        {

        }
        protected void Apply(ToastNotificationFailedEvent @event)
        {

        }
        protected void Apply(RawNotificationFailedEvent @event)
        {

        }
    }
}