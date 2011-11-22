using System;
using System.Globalization;

namespace Entile.Server.Domain
{
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