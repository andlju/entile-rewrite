using System;

namespace Entile.TestApp.Models
{
    public class ClientModel : HyperMediaModel
    {
        public Guid ClientId;
        public string NotificationUri;
        public SubscriptionModel[] Subscriptions;
    }
}