using Entile.Server;

namespace Entile.NancyHost.Api
{
    [Api("/api")]
    public class RootApiModel : ApiModelBase
    {
        public RootApiModel(IMessageDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
            : base(commandDispatcher, queryDispatcher)
        {
        }

        public object Self()
        {
            return new
                       {
                           Links = GetMethodInfos(
                               () => new ClientApiModel(null, null).Self(null),
                               () => new ClientApiModel(null, null).Register(null)
                               ).AsLinks()
                       };
        }
    }
}