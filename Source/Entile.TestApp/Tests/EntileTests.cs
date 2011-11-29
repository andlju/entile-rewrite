using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Input;
using AgFx;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Entile.TestApp.Tests
{

    public enum SubscriptionKind
    {
        Toast,
        Tile,
        Raw
    }

    public class ApiCommandBase : ICommand
    {
        private static readonly Regex ParametersRegex = new Regex(@"\{(?<param>\w+)\}");

        private IViewContext _viewContext;
        private string _uri;
        private string _method;
        private string[] _params;

        public ApiCommandBase(IViewContext viewContext, string uri, string method)
        {
            _viewContext = viewContext;
            _uri = uri;
            _method = method;
            _params = ParametersRegex.Matches(_uri).Cast<Match>().Select(m => m.Groups["param"].Value).ToArray();
        }

        public bool CanExecute(object parameter)
        {
            return _params.All(p => _viewContext.GetValue(p) != null);
        }

        public void Execute(object parameter)
        {
            var actualUri = ParametersRegex.Replace(_uri, m => _viewContext.GetValue(m.Groups["param"].Value).ToString());

            HttpWebRequest request = WebRequest.CreateHttp(actualUri);
            request.Accept = "application/json";
            request.Method = _method;
            if (request.Method != "GET")
                request.ContentType = "application/json";

            request.BeginGetResponse(ReadCallback, request);
        }

        private void ReadCallback(IAsyncResult asynchronousResult)
        {
            var request = (HttpWebRequest)asynchronousResult.AsyncState;

            var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            
            string content;
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                content = streamReader.ReadToEnd();
            }

            OnResponse((int)response.StatusCode, content);
        }

        public event EventHandler CanExecuteChanged;

        protected virtual void OnResponse(int statusCode, string response)
        {
            
        }
    }

    public class RootApiCommand : ApiCommandBase
    {
        private readonly EntileViewModel _viewModel;

        public RootApiCommand(EntileViewModel viewModel, string uri, string method)
            : base(viewModel.ViewContext, uri, method)
        {
            _viewModel = viewModel;
        }

        protected override void OnResponse(int statusCode, string response)
        {
            _viewModel.LoadLinks(new[]{new LinkModel() {Rel = "register", Uri = "http://localhost:4250", Method = "PUT" }});   
        }
    }

    public class RegisterClientCommand : ApiCommandBase
    {
        public RegisterClientCommand(EntileViewModel viewModel, string uri, string method) :
            base(viewModel.ViewContext, uri, method)
        {
        }

        protected override void OnResponse(int statusCode, string response)
        {
            
        }
    }

    public class UnsubscribeCommand : ApiCommandBase
    {
        public UnsubscribeCommand(SubscriptionViewModel subscription, string uri, string method)
            : base(subscription.ViewContext, uri, method)
        {
        }

        protected override void OnResponse(int statusCode, string response)
        {
            
        }
    }

    public class SubscriptionViewModel : ActionableObject
    {
        private readonly EntileViewContext _viewContext;

        public SubscriptionViewModel(IViewContext clientViewContext, Guid subscriptionId, SubscriptionKind kind, string paramUri) 
        {
            _viewContext = new EntileViewContext(clientViewContext);
            
            _subscriptionId = subscriptionId;
            ViewContext.SetValue("SubscriptionId", _subscriptionId);
            _kind = kind;
            _paramUri = paramUri;
        }

        protected override ICommand BuildCommand(string rel, string uri, string method)
        {
            switch(rel)
            {
                case "unsubscribe":
                    return UnsubscribeCommand = new UnsubscribeCommand(this, uri, method);
            }
            return null;
        }

        protected override void RemoveCommand(string rel)
        {
            switch (rel)
            {
                case "unsubscribe":
                    UnsubscribeCommand = null;
                    break;
            }
        }

        private Guid _subscriptionId;
        public Guid SubscriptionId
        {
            get { return _subscriptionId; }
        }

        private SubscriptionKind _kind;
        public SubscriptionKind Kind
        {
            get { return _kind; }
        }

        private string _paramUri;
        public string ParamUri
        {
            get { return _paramUri; }
        }

        public EntileViewContext ViewContext
        {
            get { return _viewContext; }
        }

        private ICommand _unsubscribeCommand;
        public ICommand UnsubscribeCommand
        {
            get { return _unsubscribeCommand; }
            set
            {
                if (_unsubscribeCommand != value)
                {
                    _unsubscribeCommand = value;
                    RaisePropertyChanged("UnsubscribeCommand");
                }
            }
        }
    }

    public class EntileViewModel : ActionableObject
    {
        private readonly EntileViewContext _viewContext = new EntileViewContext();

        public EntileViewModel()
        {
            _clientId = Guid.NewGuid();
            ViewContext.SetValue("ClientId", _clientId);
            _rootApiCommand = new RootApiCommand(this, "http://localhost:4250/api", "GET");
        }

        protected override ICommand BuildCommand(string rel, string uri, string method)
        {
            switch (rel)
            {
                case "register":
                    return RegisterClientCommand = new RegisterClientCommand(this, uri, method);
            }
            return null;
        }

        protected override void RemoveCommand(string rel)
        {
            switch (rel)
            {
                case "register":
                    RegisterClientCommand = null;
                    break;
            }
        }

        private bool _activated;
        public bool Activated
        {
            get { return _activated; }
            set
            {
                if (_activated != value)
                {
                    _activated = value;
                    RaisePropertyChanged("Activated");
                }
            }
        }

        private Guid _clientId;
        public Guid ClientId
        {
            get { return _clientId; }
        }

        private RegisterClientCommand _registerClientCommand;
        public RegisterClientCommand RegisterClientCommand
        {
            get { return _registerClientCommand; }
            set
            {
                if (_registerClientCommand != value)
                {
                    _registerClientCommand = value;
                    RaisePropertyChanged("RegisterClientCommand");
                }
            }
        }

        private RootApiCommand _rootApiCommand;
        public RootApiCommand RootApiCommand
        {
            get { return _rootApiCommand; }
            set
            {
                if (_rootApiCommand!= value)
                {
                    _rootApiCommand = value;
                    RaisePropertyChanged("RootApiCommand");
                }
            }
        }

        private ObservableCollection<SubscriptionViewModel> _subscriptions = new ObservableCollection<SubscriptionViewModel>();
        public ObservableCollection<SubscriptionViewModel> Subscriptions
        {
            get { return _subscriptions; }
            set
            {
                if (value == null) 
                    throw new ArgumentNullException("value");
                
                _subscriptions.Clear();
                foreach (var sub in value)
                {
                    _subscriptions.Add(sub);
                }
                
                // RaisePropertyChanged("Subscriptions"); 
            }
        }

        public EntileViewContext ViewContext
        {
            get { return _viewContext; }
        }
    }

    public class ActionDefinition
    {
        private string _name;
        public string Name { get { return _name; } }

    }

    public interface IViewContext
    {
        object GetValue(string key);
    }

    public class EntileViewContext : IViewContext
    {
        private readonly IViewContext _parentContext;

        public EntileViewContext()
        {
        }

        public EntileViewContext(IViewContext parentContext)
        {
            _parentContext = parentContext;
        }

        private Dictionary<string, object> _lookup = new Dictionary<string, object>();

        public object GetValue(string key)
        {
            object val;
            if (_lookup.TryGetValue(key, out val))
                return val;
            if (_parentContext != null)
                return _parentContext.GetValue(key);
            
            return null;
        }

        public void SetValue(string key, object value)
        {
            _lookup[key] = value;
        }
    }

    public class LinkModel
    {
        public string Rel;
        public string Uri;
        public string Method;
    }

    public abstract class ActionableObject : NotifyPropertyChangedBase
    {
        private ObservableCollection<ICommand> _availableCommands = new ObservableCollection<ICommand>();
        private IEnumerable<LinkModel> _linkModels = new LinkModel[0];

        public void LoadLinks(IEnumerable<LinkModel> linkModels)
        {
            // Remove all old commands
            foreach(var linkmodel in _linkModels)
            {
                RemoveCommand(linkmodel.Rel);
            }

            // Replace with new
            _linkModels = linkModels.ToArray();
            foreach (var linkModel in _linkModels)
            {
                BuildCommand(linkModel.Rel, linkModel.Uri, linkModel.Method);
            }
        }

        protected abstract ICommand BuildCommand(string rel, string uri, string method);
        protected abstract void RemoveCommand(string rel);

    }
    
    public interface IDataFetcher
    {
        
    }

    [TestClass]
    public class EntileTests : SilverlightTest
    {
        // Unless found in cache, Entile root API should be loaded
        [TestMethod]
        [Asynchronous]
        public void When_Initializing_Api_Root_Should_Be_Loaded()
        {
            EntileViewModel viewModel = new EntileViewModel();
            viewModel.RootApiCommand.Execute(null);

            viewModel.PropertyChanged += (s, e) =>
                                             {
                                                 if (e.PropertyName == "RegisterClientCommand")
                                                     TestComplete();
                                             };
        }


        // Enabling notifications should GET registration information
        // If unknown client, client registration should be PUT


    }
}