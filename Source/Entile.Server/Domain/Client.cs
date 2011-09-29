using System;
using System.Collections.Generic;
using Entile.Server.Commands;
using Entile.Server.Events;

namespace Entile.Server.Domain
{
    public class Client : Aggregate<Client>
    {
        private Guid _uniqueId;
        private bool _isActive;
        private string _notificationChannel;

        public override Guid UniqueId { get { return _uniqueId; } }

        public IDictionary<string, string> ExtendedInformation { get; private set; }

        public Client()
        {
            ExtendedInformation = new Dictionary<string, string>();

            RegisterEvent<ClientRegisteredEvent>(OnClientRegistered);
            RegisterEvent<ClientRegistrationUpdatedEvent>(OnClientRegistrationUpdated);
            RegisterEvent<ExtendedInformationItemSetEvent>(OnExtendedInformationItemSet);
            RegisterEvent<ExtendedInformationItemRemovedEvent>(OnExtendedInformationItemRemoved);
            RegisterEvent<AllExtendedInformationItemsRemovedEvent>(OnAllExtendedInformationItemsRemoved);
            RegisterEvent<ClientUnregisteredEvent>(OnClientUnregistered);
        }

        public Client(Guid uniqueId, string notificationChannel)
            : this()
        {
            ApplyEvent(new ClientRegisteredEvent(uniqueId, notificationChannel));
        }

        public void UpdateRegistration(string notificationChannel)
        {
            ApplyEvent(new ClientRegistrationUpdatedEvent(notificationChannel));
        }

        public void SetExtendedInformationItem(string key, string value)
        {
            if (!_isActive)
                throw new ClientNotRegisteredException(UniqueId);

            ApplyEvent(new ExtendedInformationItemSetEvent(key, value));
        }

        public void RemoveExtendedInformationItem(string key)
        {
            if (!_isActive)
                throw new ClientNotRegisteredException(UniqueId);

            if (!ExtendedInformation.ContainsKey(key))
                throw new InvalidExtendedInformationItemException(UniqueId, key);

            ApplyEvent(new ExtendedInformationItemRemovedEvent(key));
        }

        public void RemoveAllExtendedInformationItems()
        {
            ApplyEvent(new AllExtendedInformationItemsRemovedEvent());
        }

        public void Unregister()
        {
            if (!_isActive)
                throw new ClientNotRegisteredException(UniqueId);

            ApplyEvent(new ClientUnregisteredEvent());
        }

        public void SendNotification(NotificationBase notification, int attemptsLeft, INotificationSender notificationSender, IMessageScheduler commandScheduler)
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
                        ApplyEvent(new ToastNotificationSucceededEvent(toastNotification.NotificationId, attemptsLeft, toastNotification.Title, toastNotification.Body, toastNotification.ParamUri));
                    else if (rawNotification != null)
                        ApplyEvent(new RawNotificationSucceededEvent(rawNotification.NotificationId, attemptsLeft, rawNotification.RawContent));
                    else if (tileNotification != null)
                        ApplyEvent(new TileNotificationSucceededEvent(tileNotification.NotificationId, attemptsLeft, tileNotification.Title, tileNotification.BackgroundUri, tileNotification.Counter, tileNotification.BackTitle, tileNotification.BackContent, tileNotification.BackBackgroundUri, tileNotification.ParamUri));
                    return;
                }
            }

            // Something went wrong somehow. First, let's publish an event saing so.
            if (toastNotification != null)
                ApplyEvent(new ToastNotificationFailedEvent(toastNotification.NotificationId, attemptsLeft, DateTime.Now, response.HttpStatusCode, response.NotificationStatus, response.DeviceConnectionStatus, response.SubscriptionStatus, toastNotification.Title, toastNotification.Body, toastNotification.ParamUri));
            else if (rawNotification != null)
                ApplyEvent(new RawNotificationFailedEvent(rawNotification.NotificationId, attemptsLeft, DateTime.Now, response.HttpStatusCode, response.NotificationStatus, response.DeviceConnectionStatus, response.SubscriptionStatus, rawNotification.RawContent));
            else if (tileNotification != null)
                ApplyEvent(new TileNotificationFailedEvent(tileNotification.NotificationId, attemptsLeft, DateTime.Now, response.HttpStatusCode, response.NotificationStatus, response.DeviceConnectionStatus, response.SubscriptionStatus, tileNotification.Title, tileNotification.BackgroundUri, tileNotification.Counter, tileNotification.BackTitle, tileNotification.BackContent, tileNotification.BackBackgroundUri, tileNotification.ParamUri));

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
                    commandScheduler.ScheduleMessage(new SendToastNotificationCommand(this.UniqueId, toastNotification.NotificationId, toastNotification.Title, toastNotification.Body, toastNotification.ParamUri, attemptsLeft), resendTime);
                else if (rawNotification != null)
                    commandScheduler.ScheduleMessage(new SendRawNotificationCommand(this.UniqueId, rawNotification.NotificationId, rawNotification.RawContent, attemptsLeft), resendTime);
                else if (tileNotification != null)
                    commandScheduler.ScheduleMessage(new SendTileNotificationCommand(this.UniqueId, tileNotification.NotificationId, tileNotification.ParamUri, tileNotification.Title, tileNotification.Counter, tileNotification.BackgroundUri, tileNotification.BackTitle, tileNotification.BackContent, tileNotification.BackBackgroundUri, attemptsLeft), resendTime);
            }

        }

        private void OnClientRegistered(ClientRegisteredEvent @event)
        {
            _uniqueId = @event.UniqueId;
            _isActive = true;
            _notificationChannel = @event.NotificationChannel;
        }

        private void OnClientRegistrationUpdated(ClientRegistrationUpdatedEvent @event)
        {
            _isActive = true;
            _notificationChannel = @event.NotificationChannel;
        }

        private void OnExtendedInformationItemSet(ExtendedInformationItemSetEvent @event)
        {
            ExtendedInformation[@event.Key] = @event.Value;
        }

        private void OnExtendedInformationItemRemoved(ExtendedInformationItemRemovedEvent @event)
        {
            ExtendedInformation.Remove(@event.Key);
        }

        private void OnAllExtendedInformationItemsRemoved(AllExtendedInformationItemsRemovedEvent @event)
        {
            ExtendedInformation.Clear();
        }

        private void OnClientUnregistered(ClientUnregisteredEvent @event)
        {
            _isActive = false;
        }
    }
}