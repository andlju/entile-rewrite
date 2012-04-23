using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Entile.TestApp.Models;
using Entile.TestApp.Utilities;
using Entile.TestApp.ViewModels;
using Newtonsoft.Json;

namespace Entile.TestApp.Actions
{
    public class LinkResponseEventArgs : EventArgs
    {
        public IEnumerable<LinkModel> Links { get; private set; }

        public LinkResponseEventArgs(IEnumerable<LinkModel> links)
        {
            Links = links;
        }
    }

    public class ApiCommandBase : ApiQueryBase
    {
        public class FormDefinition
        {
            public string Action;
            public Dictionary<string, string> Form;
        }

        public ApiCommandBase(IViewContext viewContext, string uri) : base(viewContext, uri)
        {
        }

        protected override void OnResponse(string uri, int statusCode, string response)
        {
            JsonSerializer serializer = new JsonSerializer();
            var formDef = serializer.Deserialize<FormDefinition>(new JsonTextReader(new StringReader(response)));

            var keys = formDef.Form.Keys.ToArray();
            foreach(var key in keys)
            {
                formDef.Form[key] = ViewContext.GetValue(key).ToString();
            }

            var webClient = WebClientFactory.CreateWebClient();
            webClient.ResponseCompleted += CommandResponseCompleted;
            var formData = new StringWriter();
            serializer.Serialize(formData, formDef.Form);
            webClient.SendRequest(uri, formDef.Action, formData.ToString());
        }

        void CommandResponseCompleted(object sender, WebResponseEventArgs e)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class ApiQueryBase : ICommand
    {
        public static IWebClientFactory WebClientFactory = new DefaultWebClientFactory();

        private static readonly Regex ParametersRegex = new Regex(@"\{(?<param>\w+)\}");

        private IViewContext _viewContext;
        private string _uri;
        private string[] _params;

        protected ApiQueryBase(IViewContext viewContext, string uri)
        {
            _viewContext = viewContext;
            _uri = uri;
            _params = ParametersRegex.Matches(_uri).Cast<Match>().Select(m => m.Groups["param"].Value).ToArray();
        }

        public IViewContext ViewContext
        {
            get { return _viewContext; }
        }

        public bool CanExecute(object parameter)
        {
            return _params.All(p => ViewContext.GetValue(p) != null);
        }

        public void Execute(object parameter)
        {
            var webClient = WebClientFactory.CreateWebClient();
            var actualUri = ParametersRegex.Replace(_uri, m => ViewContext.GetValue(m.Groups["param"].Value).ToString());
            webClient.ResponseCompleted += QueryResponseCompleted;
            webClient.SendRequest(actualUri, "GET", null);
        }

        void QueryResponseCompleted(object sender, WebResponseEventArgs e)
        {
            OnResponse(e.Uri, e.StatusCode, e.Content);
        }

        public event EventHandler CanExecuteChanged;

        protected abstract void OnResponse(string uri, int statusCode, string response);
    }
}