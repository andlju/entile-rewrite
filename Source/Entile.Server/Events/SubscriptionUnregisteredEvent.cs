using System;

namespace Entile.Server.Events
{
    public class SubscriptionUnregisteredEvent : EventBase
    {
        public readonly Guid SubscriptionId;

        public SubscriptionUnregisteredEvent(Guid subscriptionId)
        {
            SubscriptionId = subscriptionId;
        }
    }
}