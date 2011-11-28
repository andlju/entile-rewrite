using System.Collections.Generic;
using Entile.NancyHost.Api.ViewModel;
using Entile.Server;
using Entile.Server.Commands;

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
            var links = new List<LinkViewModel>();
            links.AddRange(ToLinks(typeof(RootApiModel), null));
            links.AddRange(ToEntrypointLinks(typeof(ClientApiModel), null));
            links.AddRange(ToEntrypointLinks(typeof(ClientsAdminApiModel), null));

            return new
                       {
                           Links = links
                       };
        }
        
        [ApiMethod(HttpMethod = "PUT")]
        public object Register(RegisterClientCommand command)
        {
            DispatchCommand(command);

            return new
            {
                Links = ToLinks(typeof(ClientApiModel), command)
            };
        }
    }
}