using System;

namespace Entile.Server.Commands
{
    public class UnregisterSubscriptionCommand : CommandBase
    {
        public Guid ClientId;
        public Guid SubscriptionId;

        public UnregisterSubscriptionCommand()
        {
            
        }

        public UnregisterSubscriptionCommand(Guid clientId, Guid subscriptionId)
        {
            ClientId = clientId;
            SubscriptionId = subscriptionId;
        }
    }
}