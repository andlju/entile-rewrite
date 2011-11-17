using Entile.Server;
using Entile.Server.Commands;
using Entile.Server.Queries;

namespace Entile.NancyHost.Api
{
    [Api("/api/client/{clientId}")]
    public class ClientApiModel : ApiModelBase
    {
        public ClientApiModel(IMessageDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
            : base(commandDispatcher, queryDispatcher)
        {
        }

        public object Self(GetClientQuery query)
        {
            dynamic client = QueryDispatcher.Invoke(query);
            foreach(dynamic sub in client.Subscriptions)
            {
                var subscriptionId = sub.SubscriptionId;
                sub.Links = typeof (SubscriptionApiModel).ToRootLink();
            }
            return new
                       {
                           Client = client,
                           Links = typeof (ClientApiModel).ToLinks()
                       };
        }

        public object Register(RegisterClientCommand command)
        {
            CommandDispatcher.Dispatch(command);

            return new
                       {
                           Links = typeof (ClientApiModel).ToLinks()
                       };
        }

        public object Unregister(UnregisterClientCommand command)
        {
            CommandDispatcher.Dispatch(command);

            return new
                       {
                           Links = typeof (RootApiModel).ToLinks()
                       };
        }

        public object Subscribe(SubscribeCommand command)
        {
            CommandDispatcher.Dispatch(command);

            return new
                       {
                           Links = typeof (SubscriptionApiModel).ToLinks()
                       };
        }
    }
}