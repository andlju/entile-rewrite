using System.Web.Http;
using Entile.WebApiHost.Models;

namespace Entile.WebApiHost.Controllers
{
    public class RootController : ApiController
    {
        public RootResponse Get()
        {
            var root = new RootResponse();
            RootHyperMediaProvider.Instance.AddHyperMedia(Request, root);
            return root;
        }
    }
}