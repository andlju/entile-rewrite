using System;
using System.Collections.Generic;
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

    public class LinkDefinition
    {
        public string Rel { get; set; }
        public Uri Uri { get; set; }
    }

    public class CommandDefinition
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Method { get; set; }
        public FieldDefinition[] Fields { get; set; }
    }

    public class FieldDefinition
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Optional { get; set; }
    }

    public class HyperMediaResponse
    {
        public HyperMediaResponse()
        {
            Links = new List<LinkDefinition>();
            Commands = new List<CommandDefinition>();
        }

        public List<LinkDefinition> Links { get; set; }
        public List<CommandDefinition> Commands { get; set; } 
    }

    public class ClientResponse : HyperMediaResponse
    {
        public string NotificationUri { get; set; }
    }


    public class ClientsController : ApiController
    {
        private readonly IMessageDispatcher _dispatcher;

        public ClientsController()
        {
            _dispatcher = Bootstrapper.CurrentServer.CommandDispatcher;
        }

        // GET /api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET /api/clients/a42076d0-9728-48d9-ba6b-c6ed722fb49e
        public ClientResponse Get(Guid id)
        {
            var queries = new ClientQueries();
            var client = queries.GetClient(new GetClientQuery() {ClientId = id});

            var response = new ClientResponse()
                               {
                                   NotificationUri = client.NotificationChannel
                               };

            return response;
        }

        // POST /api/clients
        public HttpResponseMessage<HyperMediaResponse> Post(ClientResource client)
        {
            var id = Guid.NewGuid();
            var command = new RegisterClientCommand() { ClientId = id, NotificationChannel = client.NotificationUri };
            _dispatcher.Dispatch(command);
            var clientUri = new Uri(Request.RequestUri, "/api/clients/" + id);
            
            var hyperMediaResponse = new HyperMediaResponse();
            hyperMediaResponse.Links.Add(new LinkDefinition() { Rel = "self", Uri = clientUri});

            var response = new HttpResponseMessage<HyperMediaResponse>(hyperMediaResponse, HttpStatusCode.Created);
            response.Headers.Location = clientUri;
            
            return response;
        }

        public HttpResponseMessage<HyperMediaResponse> Post(Guid id, ClientResource client)
        {
            var command = new RegisterClientCommand() { ClientId = id, NotificationChannel = client.NotificationUri };
            _dispatcher.Dispatch(command);
            var clientUri = new Uri(Request.RequestUri, "/api/clients/" + id);

            var hyperMediaResponse = new HyperMediaResponse();
            hyperMediaResponse.Links.Add(new LinkDefinition() { Rel = "self", Uri = clientUri });

            var response = new HttpResponseMessage<HyperMediaResponse>(hyperMediaResponse, HttpStatusCode.Accepted);
            response.Headers.Location = clientUri;

            return response;
        }

        // DELETE /api/clients/a42076d0-9728-48d9-ba6b-c6ed722fb49e
        public void Delete(Guid id)
        {
            var command = new UnregisterClientCommand(id);
            _dispatcher.Dispatch(command);
        }
    }
}