using System;

namespace Entile.Server.Queries
{
    public class GetSubscriptionQuery : IMessage
    {
        public Guid ClientId;
        public Guid SubscriptionId;
    }
}