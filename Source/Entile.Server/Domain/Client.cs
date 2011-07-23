using System;
using System.Collections.Generic;
using Entile.Server.Events;

namespace Entile.Server.Domain
{
    public interface INotificationSender
    {
        NotificationResponse SendNotification(string channel, string title, string body);
    }
    
    public class NotificationResponse
    {
        public string Status { get; private set; }

        public NotificationResponse(string status)
        {
            Status = status;
        }
    }

    public class Client : Aggregate<Client>
    {
        private string _uniqueId;
        private bool _isActive;
        private string _notificationChannel;

        public override string UniqueId { get { return _uniqueId; } }

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

        public Client(string uniqueId, string notificationChannel)
            : this()
        {
            ApplyEvent(new ClientRegisteredEvent(uniqueId, notificationChannel));
        }

        public void UpdateRegistration(string notificationChannel)
        {
            if (_isActive)
            {
                ApplyEvent(new ClientRegistrationUpdatedEvent(notificationChannel));
            }
            else
            {
                ApplyEvent(new ClientReregisteredEvent(UniqueId, notificationChannel));
            }
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

        public void SendNotification(string title, string body, INotificationSender notificationSender)
        {
            var response = notificationSender.SendNotification(_notificationChannel, title, body);

            if (response.Status == "Failed")
            {
                ApplyEvent(new NotificationFailedEvent(title, body));
            }
            else
            {
                ApplyEvent(new NotificationSucceededEvent(title, body));
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