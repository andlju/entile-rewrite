using System;
using System.Collections.Generic;
using Entile.Server.Commands;
using Entile.Server.Domain;

namespace Entile.Server.Events
{
    public class SubscribedEvent : EventBase
    {
        public readonly Guid SubscriptionId;
        public readonly NotificationKind Kind;
        public readonly string ParamUri;
        public readonly Dictionary<string, string> ExtendedInformation;

        public SubscribedEvent(Guid subscriptionId, NotificationKind kind, string paramUri, Dictionary<string, string> extendedInformation)
        {
            SubscriptionId = subscriptionId;
            Kind = kind;
            ParamUri = paramUri;
            ExtendedInformation = extendedInformation;
        }
    }
}