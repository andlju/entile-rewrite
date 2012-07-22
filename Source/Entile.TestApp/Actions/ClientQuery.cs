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
            base.OnResponse(uri, statusCode, response);

            var jsonSerializer = new JsonSerializer();

            var clientModel = jsonSerializer.Deserialize<ClientModel>(new JsonTextReader(new StringReader(response)));
        }

    }
}