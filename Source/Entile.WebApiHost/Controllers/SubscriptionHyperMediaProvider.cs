using System.Collections.Generic;
using Entile.Server.Commands;
using Entile.WebApiHost.Models;

namespace Entile.WebApiHost.Controllers
{
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