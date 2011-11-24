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
            var links = ToEntrypointLinks(typeof (ClientApiModel), null);
            links.AddRange(ToEntrypointLinks(typeof(ClientsAdminApiModel), null));

            return new
                       {
                           Links = links
                       };
        }
    }
}