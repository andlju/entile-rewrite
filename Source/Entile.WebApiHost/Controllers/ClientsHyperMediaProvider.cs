using System.Collections.Generic;
using System.Net.Http;
using Entile.Server.Commands;

namespace Entile.WebApiHost.Controllers
{
    public class RootHyperMediaProvider : HyperMediaProviderBase<RootResponse>
    {
        public static RootHyperMediaProvider Instance = new RootHyperMediaProvider();

        protected override IEnumerable<LinkBuilder> Links(RootResponse response)
        {
            yield return Link("/api", "self");
        }

        protected override IEnumerable<CommandBuilder> Commands(RootResponse response)
        {
            yield return Command<RegisterClientCommand>("/api/clients", "register", "Register client", "POST") ;
        }
    }

    public class ClientHyperMediaProvider : HyperMediaProviderBase<ClientResponse>
    {
        public static ClientHyperMediaProvider Instance = new ClientHyperMediaProvider();

        protected override IEnumerable<LinkBuilder> Links(ClientResponse response)
        {
            yield return Link("/api", "root");
            yield return Link("/api/clients/{clientId}", "self");
            yield return Link("/api/clients/{clientId}/subscriptions", "subscriptions");
        }

        protected override IEnumerable<CommandBuilder> Commands(ClientResponse response)
        {
            yield return Command("/api/clients/{clientId}", "unregister", "Unregister client", "DELETE");
            yield return Command<SubscribeCommand>("/api/clients/{clientId}/subscriptions", "subscribe", "Add subscription");
        }
    }

    public class SubscriptionHyperMediaProvider: HyperMediaProviderBase<SubscriptionResponse>
    {
        protected override IEnumerable<LinkBuilder> Links(SubscriptionResponse response)
        {
            yield return Link("/api/clients/{clientId}/subscriptions/{subscriptionId}", "self");
            yield return Link("/api/clients/{clientId}", "client");
        }

        protected override IEnumerable<CommandBuilder> Commands(SubscriptionResponse response)
        {
            yield return Command<UnsubscribeCommand>(
                "/api/clients/{clientId}/subscriptions/{subscriptionId}", "unsubscribe",
                "Unsubscribe from notifications", "DELETE");
        }
    }
}