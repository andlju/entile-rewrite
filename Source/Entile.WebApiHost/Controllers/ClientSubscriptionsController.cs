using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Entile.Server;
using Entile.Server.Commands;
using Entile.Server.Queries;
using Entile.Server.QueryHandlers;
using Entile.WebApiHost.Models;

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

        public SubscriptionResponse Get(Guid clientId, Guid subscriptionId)
        {
            var queries = new ClientQueries();
            var query = new GetSubscriptionQuery() {ClientId = clientId, SubscriptionId = subscriptionId};
            var subscription = queries.GetSubscription(query);
            if (subscription == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

            var response = new SubscriptionResponse()
                               {
                                   ClientId = subscription.ClientId,
                                   SubscriptionId = subscription.SubscriptionId,
                                   NotificationKind =
                                       ((Server.Domain.NotificationKind) subscription.NotificationKind).ToString(),
                                   ParamUri = subscription.ParamUri,
                                   ExtendedInformation = subscription.ExtendedInformation.ToDictionary(ei => ei.Key, ei => ei.Value)
                               };
            _applier.Apply(Request, response);

            return response;
        }

        public HttpResponseMessage Post(Guid clientId, SubscribeCommand command)
        {
            var subscriptionId = Guid.NewGuid();
            command.ClientId = clientId;
            command.SubscriptionId = subscriptionId;

            _dispatcher.Dispatch(command);

            var response = new HttpResponseMessage(HttpStatusCode.Created);
            var locationUri = _applier.GetLink("self", Request, new SubscriptionResponse() { ClientId = clientId, SubscriptionId = subscriptionId});

            response.Headers.Location = locationUri;

            return response;
        }

        public HttpResponseMessage Delete(Guid clientId, Guid subscriptionId, UnsubscribeCommand command)
        {
            command.ClientId = clientId;
            command.SubscriptionId = subscriptionId;
            _dispatcher.Dispatch(command);

            var response = new HttpResponseMessage(HttpStatusCode.Accepted);
            var locationUri = _applier.GetLink("client", Request, new SubscriptionResponse() { ClientId = clientId, SubscriptionId = subscriptionId });
            
            response.Headers.Location = locationUri;

            return response;

        }
    }
}