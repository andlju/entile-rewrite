using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Entile.Server;
using Entile.Server.Commands;

namespace Entile.WebApiHost.Controllers
{
    public class ClientSubscriptionsController : ApiController
    {
        private readonly IMessageDispatcher _dispatcher;
        private readonly HyperMediaApplier _applier;

        public ClientSubscriptionsController()
        {
            _dispatcher = Bootstrapper.CurrentServer.CommandDispatcher;
            _applier = new HyperMediaApplier();
            _applier.RegisterProvider(new SubscriptionHyperMediaProvider());
        }

        public HttpResponseMessage Post(Guid clientId, SubscriptionResource subscription)
        {
            var subscriptionId = Guid.NewGuid();
            var command = new SubscribeCommand(
                clientId,
                subscriptionId,
                (Server.Domain.NotificationKind)Enum.Parse(typeof (Server.Domain.NotificationKind),
                                                           subscription.NotificationKind), 
                subscription.ParamUri, subscription.ExtendedInfo);

            _dispatcher.Dispatch(command);
            
            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}