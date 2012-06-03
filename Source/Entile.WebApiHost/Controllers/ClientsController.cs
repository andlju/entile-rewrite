using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Entile.Server;
using Entile.Server.Commands;
using Entile.Server.Queries;
using Entile.Server.QueryHandlers;
using Entile.WebApiHost.Models;

namespace Entile.WebApiHost.Controllers
{

    public class NotificationController : ApiController
    {
        public List<SubscriptionResponse> Get()
        {
            var queries = new SubscriptionQueries();
            var subs = queries.ListSubscriptions(new ListSubscriptionsQuery());
            return null;
        }
    }

    public class ClientsController : ApiController
    {
        private readonly IMessageDispatcher _dispatcher;
        private readonly HyperMediaApplier _applier;

        public ClientsController()
        {
            _dispatcher = Bootstrapper.CurrentServer.CommandDispatcher;
            _applier = new HyperMediaApplier();
            _applier.RegisterProvider(new ClientHyperMediaProvider());
            _applier.RegisterProvider(new SubscriptionHyperMediaProvider());
        }

        // GET /api/clients/a42076d0-9728-48d9-ba6b-c6ed722fb49e
        public ClientResponse Get(Guid clientId)
        {
            var queries = new ClientQueries();
            var client = queries.GetClient(new GetClientQuery() { ClientId = clientId });
            if (client == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

            var response = new ClientResponse()
                               {
                                   ClientId = client.ClientId,
                                   NotificationUri = client.NotificationChannel,
                                   Subscriptions = client.Subscriptions.Select(s => new SubscriptionResponse()
                                                                                        {
                                                                                            ClientId = client.ClientId,
                                                                                            SubscriptionId = s.SubscriptionId,
                                                                                            NotificationKind = ((Server.Domain.NotificationKind)s.NotificationKind).ToString(),
                                                                                            ParamUri = s.ParamUri
                                                                                        }).ToArray()
                               };
            _applier.Apply(Request, response);
            foreach(var sub in response.Subscriptions)
                _applier.Apply(Request, sub);

            return response;
        }

        // POST /api/clients
        public HttpResponseMessage Post(RegisterClientCommand command)
        {
            var id = Guid.NewGuid();
            command.ClientId = id;

            _dispatcher.Dispatch(command);
            
            var locationUri = _applier.GetLink("self", Request, new ClientResponse() {ClientId = command.ClientId});

            var response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Headers.Location = locationUri;
            
            return response;
        }

        // POST /api/clients/a42076d0-9728-48d9-ba6b-c6ed722fb49e
        public HttpResponseMessage Post(Guid clientId, RegisterClientCommand command)
        {
            command.ClientId = clientId;
            _dispatcher.Dispatch(command);
            var locationUri = _applier.GetLink("self", Request, new ClientResponse() { ClientId = command.ClientId });

            var response = new HttpResponseMessage(HttpStatusCode.Accepted);
            response.Headers.Location = locationUri;

            return response;
        }

        // DELETE /api/clients/a42076d0-9728-48d9-ba6b-c6ed722fb49e
        public HttpResponseMessage Delete(Guid clientId, UnregisterClientCommand command)
        {
            command.ClientId = clientId;
            _dispatcher.Dispatch(command);
            
            var response = new HttpResponseMessage(HttpStatusCode.Accepted);

            var locationUri = _applier.GetLink("root", Request, new ClientResponse() {ClientId = clientId});
            response.Headers.Location = locationUri;

            return response;
        }
    }
}