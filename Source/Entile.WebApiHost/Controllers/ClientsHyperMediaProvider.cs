using System.Collections.Generic;
using System.Net.Http;
using Entile.Server.Commands;

namespace Entile.WebApiHost.Controllers
{
    public class ClientHyperMediaProvider : HyperMediaProviderBase<ClientResponse>
    {
        protected override IEnumerable<LinkDefinition> Links(HttpRequestMessage request, ClientResponse response)
        {
            yield return BuildLink(request, response, "/api/clients/{clientId}", "self");
            yield return BuildLink(request, response, "/api/clients/{clientId}/subscriptions", "subscriptions");
        }

        protected override IEnumerable<CommandDefinition> Commands(HttpRequestMessage request, ClientResponse response)
        {
            yield return BuildCommand(request, response, "/api/clients/{clientId}", "unregister", "Unregister client", "DELETE");
            yield return BuildCommand<SubscribeCommand>(request, response, "/api/clients/{clientId}/subscriptions", "subscribe", "Add subscription");
        }
    }

    public class SubscriptionHyperMediaProvider: HyperMediaProviderBase<SubscriptionResponse>
    {
        protected override IEnumerable<LinkDefinition> Links(HttpRequestMessage request, SubscriptionResponse response)
        {
            yield return BuildLink(request, response, "/api/clients/{clientId}/subscriptions/{subscriptionId}", "self");
            yield return BuildLink(request, response, "/api/clients/{clientId}", "client");
        }

        protected override IEnumerable<CommandDefinition> Commands(HttpRequestMessage request, SubscriptionResponse response)
        {
            yield return
                BuildCommand<UnsubscribeCommand>(request, response,
                                                 "/api/clients/{clientId}/subscriptions/{subscriptionId}", "unsubscribe",
                                                 "Unsubscribe from notifications", "DELETE");
        }
    }
}