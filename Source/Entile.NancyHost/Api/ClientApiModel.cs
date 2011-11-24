using Entile.NancyHost.Api.ViewModel;
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
            var client = ClientViewModel.FromView(_clientQueries.GetClient(query));
            if (client == null)
            {
                HttpStatusCode = HttpStatusCode.NotFound;
                return new
                           {
                               Links = ToLinks(typeof(RootApiModel), null)
                           };
            }

            foreach (var sub in client.Subscriptions)
            {
                sub.Links = ToEntrypointLinks(typeof(SubscriptionApiModel), sub);
            }
            client.Links = ToLinks(typeof (ClientApiModel), client);
            
            return client;
        }

        [ApiMethod(Entrypoint = true, HttpMethod = "PUT")]
        public object Register(RegisterClientCommand command)
        {
            DispatchCommand(command);

            return new
                       {
                           Links = ToLinks(typeof(ClientApiModel), command)
                       };
        }

        [ApiMethod(HttpMethod = "DELETE")]
        public object Unregister(UnregisterClientCommand command)
        {
            try
            {
                DispatchCommand(command);
            }
            catch (ClientNotRegisteredException ex)
            {
                HttpStatusCode = HttpStatusCode.NotFound;
                return new
                {
                    ex.Message,
                    Links = ToLinks(typeof(RootApiModel), null)
                };
            }
            return new
            {
                Links = ToLinks(typeof(RootApiModel), null)
            };
        }

        public object Subscribe(SubscribeCommand command)
        {
            DispatchCommand(command);

            return new
                       {
                           Links = ToLinks(typeof(SubscriptionApiModel), command)
                       };
        }
    }
}