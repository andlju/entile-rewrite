using System.Collections.Generic;
using Entile.Server.Commands;
using Entile.Server.Queries;
using Entile.WebApiHost.Models;

namespace Entile.WebApiHost.Controllers
{
    public class RootHyperMediaProvider : HyperMediaProviderBase<RootResponse>
    {
        public static RootHyperMediaProvider Instance = new RootHyperMediaProvider();

        protected override IEnumerable<LinkBuilder> Links(RootResponse response)
        {
            yield return Link("/api", "self");
        }

        protected override IEnumerable<CommandBuilder> Commands(RootResponse response)
        {
            yield return Command<RegisterClientCommand>("/api/clients", "register", "Register client", "POST") ;
        }

        protected override IEnumerable<QueryBuilder> Queries(RootResponse response)
        {
            yield return Query<GetClientQuery>("/api/clients/{clientId}", "client", "Get client");
        }
    }
}