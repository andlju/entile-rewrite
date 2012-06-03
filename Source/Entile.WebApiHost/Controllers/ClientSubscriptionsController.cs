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

        public HttpResponseMessage Post(Guid clientId, SubscribeCommand command)
        {
            var subscriptionId = Guid.NewGuid();
            command.ClientId = clientId;
            command.SubscriptionId = subscriptionId;

            _dispatcher.Dispatch(command);

            var response = new HttpResponseMessage(HttpStatusCode.Created);
            var subscriptionUri = new Uri(Request.RequestUri, "/api/clients/" + clientId + "/subscriptions/" + subscriptionId);
            response.Headers.Location = subscriptionUri;

            return response;
        }
    }
}