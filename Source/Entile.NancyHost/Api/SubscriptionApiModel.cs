using Entile.Server;
using Entile.Server.Commands;
using Entile.Server.QueryHandlers;
using Entile.Server.ViewHandlers;
using Nancy;

namespace Entile.NancyHost.Api
{
    [Api("/api/client/{clientId}/subscription/{subscriptionId}")]
    public class SubscriptionApiModel : ApiModelBase
    {
        public SubscriptionApiModel(IMessageDispatcher commandDispatcher)
            : base(commandDispatcher)
        {
        }

        public object Self()
        {
            return null;
        }

        public object Unsubscribe(UnsubscribeCommand command)
        {
            DispatchCommand(command);

            return new
                       {
                           Links = ToLinks(typeof(ClientApiModel))
                       };
        }

    }
}