using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Entile.Server;
using Entile.Server.Commands;
using Entile.Server.Queries;
using Entile.Server.QueryHandlers;

namespace Entile.WebApiHost.Controllers
{
    public class ClientResource
    {
        public string NotificationUri { get; set; }
    }

    public class ClientResponse : HyperMediaResponse
    {
        public Guid ClientId { get; set; }
        public string NotificationUri { get; set; }

        public SubscriptionResponse[] Subscriptions { get; set; }
    }

    public class SubscriptionResource
    {
        public string NotificationKind { get; set; }
        public string ParamUri { get; set; }
        public Dictionary<string, string> ExtendedInfo { get; set; }
    }

    public class SubscriptionResponse : HyperMediaResponse
    {
        public Guid ClientId { get; set; }
        public Guid SubscriptionId { get; set; }
        public string NotificationKind { get; set; }
        public string ParamUri { get; set; }
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

            var response = new ClientResponse()
                               {
                                   ClientId = client.ClientId,
                                   NotificationUri = client.NotificationChannel,
                                   Subscriptions = client.Subscriptions.Select(s => new SubscriptionResponse()
                                                                                        {
                                                                                            ClientId = client.ClientId,
                                                                                            SubscriptionId = s.SubscriptionId,
                                                                                            NotificationKind = ((Server.Domain.NotificationKind)s.NotificationKind).ToString(CultureInfo.InvariantCulture),
                                                                                            ParamUri = s.ParamUri
                                                                                        }).ToArray()
                               };
            _applier.Apply(Request, response);
            foreach(var sub in response.Subscriptions)
                _applier.Apply(Request, sub);

            return response;
        }

        // POST /api/clients
        public HttpResponseMessage Post(ClientResource client)
        {
            var id = Guid.NewGuid();
            var command = new RegisterClientCommand() { ClientId = id, NotificationChannel = client.NotificationUri };
            _dispatcher.Dispatch(command);
            var clientUri = new Uri(Request.RequestUri, "/api/clients/" + id);

            var response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Headers.Location = clientUri;
            
            return response;
        }

        // POST /api/clients/a42076d0-9728-48d9-ba6b-c6ed722fb49e
        public HttpResponseMessage Post(Guid clientId, ClientResource client)
        {
            var command = new RegisterClientCommand() { ClientId = clientId, NotificationChannel = client.NotificationUri };
            _dispatcher.Dispatch(command);
            var clientUri = new Uri(Request.RequestUri, "/api/clients/" + clientId);

            var response = new HttpResponseMessage(HttpStatusCode.Accepted);
            response.Headers.Location = clientUri;

            return response;
        }

        // DELETE /api/clients/a42076d0-9728-48d9-ba6b-c6ed722fb49e
        public void Delete(Guid clientId)
        {
            var command = new UnregisterClientCommand(clientId);
            _dispatcher.Dispatch(command);
        }
    }
}