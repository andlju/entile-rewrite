using System.Collections.Generic;
using System.Net.Http;
using Entile.Server.Commands;

namespace Entile.WebApiHost.Controllers
{
    public class RootHyperMediaProvider : HyperMediaProviderBase<RootResponse>
    {
        public static RootHyperMediaProvider Instance = new RootHyperMediaProvider();

        protected override IEnumerable<LinkBuilder> Links()
        {
            yield return Link("/api", "self");
        }

        protected override IEnumerable<CommandBuilder> Commands()
        {
            yield return Command<RegisterClientCommand>("/api/clients", "register", "Register client", "POST") ;
        }
    }

    public class ClientHyperMediaProvider : HyperMediaProviderBase<ClientResponse>
    {
        public static ClientHyperMediaProvider Instance = new ClientHyperMediaProvider();
        protected override IEnumerable<LinkBuilder> Links()
        {
            yield return Link("/api/clients/{clientId}", "self");
            yield return Link("/api/clients/{clientId}/subscriptions", "subscriptions");
        }

        protected override IEnumerable<CommandBuilder> Commands()
        {
            yield return Command("/api/clients/{clientId}", "unregister", "Unregister client", "DELETE");
            yield return Command<SubscribeCommand>("/api/clients/{clientId}/subscriptions", "subscribe", "Add subscription");
        }
    }

    public class SubscriptionHyperMediaProvider: HyperMediaProviderBase<SubscriptionResponse>
    {
        protected override IEnumerable<LinkBuilder> Links()
        {
            yield return Link("/api/clients/{clientId}/subscriptions/{subscriptionId}", "self");
            yield return Link("/api/clients/{clientId}", "client");
        }

        protected override IEnumerable<CommandBuilder> Commands()
        {
            yield return Command<UnsubscribeCommand>(
                "/api/clients/{clientId}/subscriptions/{subscriptionId}", "unsubscribe",
                "Unsubscribe from notifications", "DELETE");
        }
    }
}