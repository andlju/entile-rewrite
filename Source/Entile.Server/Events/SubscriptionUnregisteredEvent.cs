using System;

namespace Entile.Server.Events
{
    public class UnsbuscribedEvent : EventBase
    {
        public readonly Guid SubscriptionId;

        public UnsbuscribedEvent(Guid subscriptionId)
        {
            SubscriptionId = subscriptionId;
        }
    }
}