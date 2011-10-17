using System;
using System.Collections.Generic;
using Entile.Server.Commands;
using Entile.Server.Domain;

namespace Entile.Server.Events
{
    public class SubscriptionRegisteredEvent : EventBase
    {
        public readonly Guid SubscriptionId;
        public readonly NotificationKind Kind;
        public readonly string ParamUri;
        public readonly IEnumerable<KeyValuePair<string, string>> ExtendedInformation;

        public SubscriptionRegisteredEvent(Guid subscriptionId, NotificationKind kind, string paramUri, IEnumerable<KeyValuePair<string, string>> extendedInformation)
        {
            SubscriptionId = subscriptionId;
            Kind = kind;
            ParamUri = paramUri;
            ExtendedInformation = extendedInformation;
        }
    }
}