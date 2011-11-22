using Entile.Server;
using Entile.Server.Commands;
using Entile.Server.Domain;
using Entile.Server.Queries;
using Entile.Server.QueryHandlers;

namespace Entile.NancyHost.Api
{
    [Api("/api/client/{clientId}")]
    public class ClientApiModel : ApiModelBase
    {
        private readonly ClientQueries _clientQueries;

        public ClientApiModel(IMessageDispatcher commandDispatcher)
            : base(commandDispatcher)
        {
            _clientQueries = new ClientQueries();
        }

        public object Self(GetClientQuery query)
        {
            dynamic client = _clientQueries.GetClient(query);
            foreach(dynamic sub in client.Subscriptions)
            {
                var subscriptionId = sub.SubscriptionId;
                sub.Links = ToEntrypointLinks(typeof(SubscriptionApiModel));
            }
            return new
                       {
                           Client = client,
                           Links = ToLinks(typeof(ClientApiModel))
                       };
        }

        [ApiMethod(Entrypoint = true, HttpMethod = "PUT")]
        public object Register(RegisterClientCommand command)
        {
            DispatchCommand(command);

            return new
                       {
                           Links = ToLinks(typeof(ClientApiModel))
                       };
        }

        public object Unregister(UnregisterClientCommand command)
        {
            DispatchCommand(command);
            return new
            {
                Links = ToLinks(typeof(RootApiModel))
            };
                
        }

        public object Subscribe(SubscribeCommand command)
        {
            DispatchCommand(command);

            return new
                       {
                           Links = ToLinks(typeof(SubscriptionApiModel))
                       };
        }
    }
}