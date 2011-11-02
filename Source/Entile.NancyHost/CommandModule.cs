using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Entile.Server;
using Entile.Server.Commands;
using Entile.Server.ViewHandlers;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.ModelBinding;
using Nancy.Responses;
using TinyIoC;

namespace Entile.NancyHost
{
    public class Link
    {
        private string _href;
        private string _rel;
        private string _method;
        private static Regex _parameters = new Regex(@"\{(?<name>[A-Za-z0-9]*)\}");

        public Link(string href, string rel, string method)
        {
            _href = href;
            _rel = rel;
            _method = method;
        }

        private static string Replacer(IDictionary<string, object> lookup, string paramName)
        {
            object val;
            if (lookup.TryGetValue(paramName, out val))
                return val.ToString();
            return "{" + paramName + "}";
        }

        public object ToResponse(IDictionary<string, object> lookup)
        {
            var href = _parameters.Replace(_href, e => Replacer(lookup, e.Groups["name"].Value));

            return new { Href = href , Rel = _rel, Method = _method};
        }
    }
    
    public class LinkCollection
    {
        private List<Link> _links;

        public LinkCollection(params Link[] links)
        {
            _links = new List<Link>(links);
        }

        public object ToResponse(IDictionary<string, object> lookup)
        {
            return _links.Select(l => l.ToResponse(lookup));
        }
    }

    public class EntileModule : NancyModule
    {
        private LinkCollection _entileLinks = new LinkCollection(
            new Link("/client/{clientId}", "client", "GET"),
            new Link("/client/{clientId}", "register", "PUT"),
            new Link("/client/{clientId}", "unregister", "DELETE"));

        public EntileModule()
        {
            Get["/"] = _ => Response.AsJson(new { Links = _entileLinks.ToResponse(new Dictionary<string, object>())});
        }
    }

    public class ClientModule : NancyModule
    {
        private LinkCollection _registeredClientLinks = new LinkCollection(
            new Link("/client/{clientId}", "register", "PUT"),
            new Link("/client/{clientId}", "unregister", "DELETE"),
            new Link("/client/{clientId}/subscription/{subscriptionId}", "subscribe", "PUT"));

        public ClientModule(IBus commandBus)
        {
            Put["client/{clientId}"] = _ =>
                                           {
                                               RegisterClientCommand cmd = this.Bind();
                                               cmd.ClientId = Context.Parameters["clientId"];

                                               commandBus.Publish(cmd);
                                               var lookup = new Dictionary<string, object> {{"clientId", cmd.ClientId}};
                                               return Response.AsJson(
                                                   new
                                                   {
                                                       Links = _registeredClientLinks.ToResponse(lookup)
                                                   });
                                           };

            Get["client/{clientId}"] = _ =>
                                           {
                                               ClientView client;
                                               var clientId = (Guid)Context.Parameters["clientId"];
                                               using (var views = new EntileViews())
                                               {
                                                   client = (from c in views.ClientViews
                                                             where c.ClientId == clientId
                                                             select c).SingleOrDefault();
                                                   if (client == null)
                                                       return new NotFoundResponse();
                                               }

                                               var lookup = new Dictionary<string, object>
                                                                {{"clientId", client.ClientId}};

                                               return Response.AsJson(
                                                   new {
                                                           Links = _registeredClientLinks.ToResponse(lookup)
                                                       });
                                           };
        }
    }

    public class CommandBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register<IBus>((c, n) => Bootstrapper.CurrentServer.CommandBus);
        }
    }
}