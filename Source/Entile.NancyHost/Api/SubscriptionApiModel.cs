using Entile.NancyHost.Api.ViewModel;
using Entile.Server;
using Entile.Server.Commands;
using Entile.Server.Queries;
using Entile.Server.QueryHandlers;
using Entile.Server.ViewHandlers;
using Nancy;

namespace Entile.NancyHost.Api
{
    [Api("/api/client/{clientId}/subscription/{subscriptionId}")]
    public class SubscriptionApiModel : ApiModelBase
    {
        private readonly ClientQueries _clientQueries;

        public SubscriptionApiModel(IMessageDispatcher commandDispatcher)
            : base(commandDispatcher)
        {
            _clientQueries = new ClientQueries();
        }

        public object Self(GetSubscriptionQuery query)
        {
            var subscription = SubscriptionViewModel.FromView(_clientQueries.GetSubscription(query));
            if (subscription == null)
            {
                HttpStatusCode = HttpStatusCode.NotFound;
                return new
                {
                    Links = ToLinks(typeof(ClientApiModel), query)
                };
            }

            subscription.Links = ToLinks(typeof(SubscriptionApiModel), subscription);
            
            return subscription;
        }

        public object Unsubscribe(UnsubscribeCommand command)
        {
            DispatchCommand(command);

            return new
                       {
                           Links = ToLinks(typeof(ClientApiModel), command)
                       };
        }

    }
}