using Entile.Server;

namespace Entile.NancyHost.Api
{
    [Api("/api")]
    public class RootApiModel : ApiModelBase
    {
        public RootApiModel(IMessageDispatcher commandDispatcher)
            : base(commandDispatcher)
        {
        }

        public object Self()
        {
            return new
                       {
                           Links = ToEntrypointLinks(typeof(ClientApiModel), null) 
                       };
        }
    }
}