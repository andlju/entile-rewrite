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
            return new
                       {
                           Client = QueryDispatcher.Invoke(query),
                           Links = typeof (ClientApiModel).AsLinks()
                       };
        }

        public object Register(RegisterClientCommand command)
        {
            CommandDispatcher.Dispatch(command);

            return new
                       {
                           Links = typeof (ClientApiModel).AsLinks()
                       };
        }

        public object Unregister(UnregisterClientCommand command)
        {
            CommandDispatcher.Dispatch(command);

            return new
                       {
                           Links = typeof (RootApiModel).AsLinks()
                       };
        }

        public object Subscribe(SubscribeCommand command)
        {
            CommandDispatcher.Dispatch(command);

            return new
                       {
                           Links = typeof (SubscriptionApiModel).AsLinks()
                       };
        }
    }
}