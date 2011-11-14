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
        public SubscriptionApiModel(IMessageDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
            : base(commandDispatcher, queryDispatcher)
        {
        }

        public object Unsubscribe(UnsubscribeCommand command)
        {
            CommandDispatcher.Dispatch(command);

            return new
                       {
                           Links = typeof (ClientApiModel).AsLinks()
                       };
        }

    }
}