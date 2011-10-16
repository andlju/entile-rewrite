using System;
using System.Globalization;

namespace Entile.Server.Domain
{
    public class ClientNotRegisteredException : Exception
    {
        public Guid ClientId { get; private set; }

        public ClientNotRegisteredException(Guid clientId) :
            base(string.Format(CultureInfo.InvariantCulture, "Client with Id {0} was not found", clientId))
        {
            ClientId = clientId;
        }
    }

    public class UnknownSubscriptionException : Exception
    {
        public Guid ClientId { get; private set; }
        public Guid SubscriptionId { get; private set; }

        public UnknownSubscriptionException(Guid clientId, Guid subscriptionId) :
            base(string.Format(CultureInfo.InvariantCulture, "Subscription {0} was not found on client {1}", subscriptionId, clientId))
        {
            ClientId = clientId;
            SubscriptionId = subscriptionId;
        }
    }
}