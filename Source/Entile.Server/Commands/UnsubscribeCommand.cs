using System;

namespace Entile.Server.Commands
{
    public class UnsubscribeCommand : CommandBase
    {
        public Guid ClientId;
        public Guid SubscriptionId;

        public UnsubscribeCommand()
        {
            
        }

        public UnsubscribeCommand(Guid clientId, Guid subscriptionId)
        {
            ClientId = clientId;
            SubscriptionId = subscriptionId;
        }
    }
}