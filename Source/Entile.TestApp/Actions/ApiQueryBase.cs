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

    public class CommandResponseEventArgs : EventArgs
    {
        public IEnumerable<CommandModel> Commands { get; private set; }

        public CommandResponseEventArgs(IEnumerable<CommandModel> commands)
        {
            Commands = commands;
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


        protected virtual void OnResponse(string uri, int statusCode, string response)
        {
            var jsonSerializer = new JsonSerializer();
            var hyperMediaModel = jsonSerializer.Deserialize<HyperMediaModel>(new JsonTextReader(new StringReader(response)));

            if (LinksReturned != null && hyperMediaModel.Links != null)
                LinksReturned(this, new LinkResponseEventArgs(hyperMediaModel.Links));

            if (CommandsReturned != null && hyperMediaModel.Commands != null)
                CommandsReturned(this, new CommandResponseEventArgs(hyperMediaModel.Commands));
        }

        public event EventHandler<LinkResponseEventArgs> LinksReturned;
        public event EventHandler<CommandResponseEventArgs> CommandsReturned;
    }
}