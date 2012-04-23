using System;

namespace Entile.TestApp.Models
{
    public class ClientModel
    {
        public Guid ClientId;
        public string NotificationChannel;
        public SubscriptionModel[] Subscriptions;
        public LinkModel[] Links;
    }
}