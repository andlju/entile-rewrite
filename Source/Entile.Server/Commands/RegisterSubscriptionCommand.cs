using System;
using System.Collections.Generic;
using Entile.Server.Domain;

namespace Entile.Server.Commands
{
    public class RegisterSubscriptionCommand : CommandBase
    {
        public readonly Guid ClientId;
        public readonly Guid SubscriptionId;
        public readonly NotificationKind Kind;
        public readonly string Uri;
        public readonly IEnumerable<KeyValuePair<string, string>> ExtendedInformation;

        public RegisterSubscriptionCommand(Guid clientId, Guid subscriptionId, NotificationKind kind, string uri, IEnumerable<KeyValuePair<string, string>> extendedInformation)
        {
            ClientId = clientId;
            SubscriptionId = subscriptionId;
            Kind = kind;
            Uri = uri;
            ExtendedInformation = extendedInformation;
        }
    }
}