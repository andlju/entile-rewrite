using System;
using System.Collections.Generic;
using Entile.Server.Domain;

namespace Entile.Server.Commands
{
    public class SubscribeCommand : CommandBase
    {
        public Guid ClientId;
        public Guid SubscriptionId;
        public NotificationKind Kind;
        public string Uri;
        public Dictionary<string, string> ExtendedInformation;

        public SubscribeCommand()
        {

        }

        public SubscribeCommand(Guid clientId, Guid subscriptionId, NotificationKind kind, string uri, Dictionary<string, string> extendedInformation)
        {
            ClientId = clientId;
            SubscriptionId = subscriptionId;
            Kind = kind;
            Uri = uri;
            ExtendedInformation = extendedInformation;
        }
    }
}