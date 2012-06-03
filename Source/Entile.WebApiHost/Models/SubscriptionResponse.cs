using System;
using System.Collections.Generic;
using Entile.WebApiHost.Controllers;

namespace Entile.WebApiHost.Models
{
    public class SubscriptionResponse : HyperMediaResponse
    {
        public Guid ClientId { get; set; }
        public Guid SubscriptionId { get; set; }
        public string NotificationKind { get; set; }
        public string ParamUri { get; set; }
        public Dictionary<string, string> ExtendedInformation { get; set; }
    }
}