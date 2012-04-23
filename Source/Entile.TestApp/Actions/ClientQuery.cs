using System;
using System.IO;
using Entile.TestApp.Models;
using Entile.TestApp.ViewModels;
using Newtonsoft.Json;

namespace Entile.TestApp.Actions
{
    public class ClientQuery : ApiQueryBase
    {
        public ClientQuery(IViewContext viewContext, string uri) : base(viewContext, uri)
        {
        }

        protected override void OnResponse(string uri, int statusCode, string response)
        {
            var jsonSerializer = new JsonSerializer();

            var clientModel = jsonSerializer.Deserialize<ClientModel>(new JsonTextReader(new StringReader(response)));

            if (LinksReturned != null)
                LinksReturned(this, new LinkResponseEventArgs(clientModel.Links));
            if (ClientUpdated != null)
                ClientUpdated(this, new ClientUpdatedEventArgs(clientModel));
        }

        public event EventHandler<LinkResponseEventArgs> LinksReturned;

        public event EventHandler<ClientUpdatedEventArgs> ClientUpdated;

    }
}