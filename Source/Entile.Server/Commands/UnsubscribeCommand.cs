using System;
using System.ComponentModel.DataAnnotations;

namespace Entile.Server.Commands
{
    public class UnsubscribeCommand : CommandBase
    {
        [Key]
        public Guid ClientId { get; set; }
        [Key]
        public Guid SubscriptionId { get; set; }

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