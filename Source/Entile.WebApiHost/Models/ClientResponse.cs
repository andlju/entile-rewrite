using System;
using Entile.WebApiHost.Controllers;

namespace Entile.WebApiHost.Models
{
    public class ClientResponse : HyperMediaResponse
    {
        public Guid ClientId { get; set; }
        public string NotificationUri { get; set; }

        public SubscriptionResponse[] Subscriptions { get; set; }
    }
}