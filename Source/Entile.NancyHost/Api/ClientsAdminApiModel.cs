using System.Linq;
using Entile.NancyHost.Api.ViewModel;
using Entile.Server;
using Entile.Server.Queries;
using Entile.Server.QueryHandlers;

namespace Entile.NancyHost.Api
{
    [Api("/api/admin/clients")]
    public class ClientsAdminApiModel : ApiModelBase
    {
        private readonly ClientQueries _clientQueries;

        public ClientsAdminApiModel(IMessageDispatcher commandDispatcher) : base(commandDispatcher)
        {
            _clientQueries = new ClientQueries();
        }

        public object Self()
        {
            var clients = _clientQueries.GetClients().Select(c => ClientViewModel.FromView(c)).ToList();
            foreach(var client in clients)
            {
                client.Links = ToEntrypointLinks(typeof (ClientApiModel), client);
                foreach(var sub in client.Subscriptions)
                {
                    sub.Links = ToEntrypointLinks(typeof (SubscriptionApiModel), sub);
                }
            }

            return clients;
        }

    }

}