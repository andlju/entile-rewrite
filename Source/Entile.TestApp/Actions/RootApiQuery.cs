using System;
using System.IO;
using Entile.TestApp.Models;
using Entile.TestApp.ViewModels;
using Newtonsoft.Json;

namespace Entile.TestApp.Actions
{
    public class RootApiQuery : ApiQueryBase
    {

        public RootApiQuery(IViewContext viewContext, string uri)
            : base(viewContext, uri)
        {
        }

        protected override void OnResponse(string uri, int statusCode, string response)
        {
            var jsonSerializer = new JsonSerializer();
            var rootApiModel = jsonSerializer.Deserialize<ClientModel>(new JsonTextReader(new StringReader(response)));
                
            if (LinksReturned != null)
                LinksReturned(this, new LinkResponseEventArgs(rootApiModel.Links));
        }

        public event EventHandler<LinkResponseEventArgs> LinksReturned;

    }
}