using System;

namespace Entile.Server.Commands
{
    public class UnregisterSubscriptionCommand : CommandBase
    {
        public readonly Guid ClientId;
        public readonly Guid SubscriptionId;

        public UnregisterSubscriptionCommand(Guid clientId, Guid subscriptionId)
        {
            ClientId = clientId;
            SubscriptionId = subscriptionId;
        }
    }
}