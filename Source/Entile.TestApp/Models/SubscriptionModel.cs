using System;
using System.Collections.Generic;

namespace Entile.TestApp.Models
{
    public class SubscriptionModel
    {
        public Guid SubscriptionId;
        public int NotificationKind;
        public string ParamUri;
        public Dictionary<string, string> ExtendedInformation;
        public LinkModel[] Links;
    }
}