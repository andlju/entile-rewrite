using System.Collections.Generic;
using System.Net.Http;
using Entile.Server.Commands;
using Entile.WebApiHost.Models;

namespace Entile.WebApiHost.Controllers
{
    public class ClientHyperMediaProvider : HyperMediaProviderBase<ClientResponse>
    {
        public static ClientHyperMediaProvider Instance = new ClientHyperMediaProvider();

        protected override IEnumerable<LinkBuilder> Links(ClientResponse response)
        {
            yield return Link("/api", "root");
            yield return Link("/api/clients/{clientId}", "self");
        }

        protected override IEnumerable<CommandBuilder> Commands(ClientResponse response)
        {
            yield return Command<RegisterClientCommand>("/api/clients/{clientId}", "reregister", "Update registration", "POST");
            yield return Command<UnregisterClientCommand>("/api/clients/{clientId}", "unregister", "Unregister client", "DELETE");
            yield return Command<SubscribeCommand>("/api/clients/{clientId}/subscriptions", "subscribe", "Add subscription");
        }
    }
}