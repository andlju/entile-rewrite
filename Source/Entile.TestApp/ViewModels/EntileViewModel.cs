using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Entile.TestApp.Actions;
using Entile.TestApp.Models;

namespace Entile.TestApp.ViewModels
{

    public class EntileViewModel : ViewModelBase
    {
        private readonly EntileViewContext _viewContext = new EntileViewContext();

        public EntileViewModel()
        {
            _clientId = Guid.Parse("fd40122f-9b15-44ed-af69-6c70eb9cb24a"); // TODO Get Id from state
            _viewContext.SetValue("ClientId", _clientId);

            RegisterLinkAction("root", l =>
                                       {
                                           var action = new RootApiQuery(_viewContext, l.Uri);
                                           RootApiQuery = action;
                                       });

            RegisterLinkAction("client", l =>
                                             {
                                                 var action = new ClientQuery(_viewContext, l.Uri);
                                                 ClientQuery = action;
                                             });

            RegisterCommandAction("register", l =>
                                           {
                                               var action = new RegisterClientCommand(_viewContext, l.Uri);
                                               RegisterClientCommand = action;
                                           });

            RegisterCommandAction("subscribe", l =>
                                                   {
                                                       var action = new SubscribeCommand(_viewContext, l.Uri);
                                                       SubscribeCommand = action;
                                                   });

            UpdateLinks(new[] { new LinkModel() { Rel = "root", Uri = "http://localhost:6776/api/" }, });
        }

        public void Initialize()
        {
            if (RootApiQuery == null)
                throw new InvalidOperationException("No root query set");

            RootApiQuery.Execute(null);
        }

        private void LinksReturned(object sender, LinkResponseEventArgs e)
        {
            UpdateLinks(e.Links);
        }

        private void CommandsReturned(object sender, CommandResponseEventArgs e)
        {
            UpdateCommands(e.Commands);
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

        private RootApiQuery _rootApiQuery;
        public RootApiQuery RootApiQuery
        {
            get { return _rootApiQuery; }
            set
            {
                if (_rootApiQuery != null)
                {
                    _rootApiQuery.LinksReturned += LinksReturned;
                    _rootApiQuery.CommandsReturned += CommandsReturned;
                }
                _rootApiQuery = value;
                _rootApiQuery.LinksReturned += LinksReturned;
                _rootApiQuery.CommandsReturned += CommandsReturned;
                RaisePropertyChanged("RootApiQuery");
            }
        }

        private ClientQuery _clientQuery;
        public ClientQuery ClientQuery
        {
            get { return _clientQuery; }
            set
            {
                if (_clientQuery != null)
                {
                    _clientQuery.LinksReturned -= LinksReturned;
                    _clientQuery.CommandsReturned -= CommandsReturned;
                }
                _clientQuery = value;
                _clientQuery.LinksReturned += LinksReturned;
                _clientQuery.CommandsReturned += CommandsReturned;
                RaisePropertyChanged("ClientQuery");
            }
        }

        private RegisterClientCommand _registerClientCommand;
        public RegisterClientCommand RegisterClientCommand
        {
            get { return _registerClientCommand; }
            set
            {
                if (_registerClientCommand != null)
                {
                    
                }
                _registerClientCommand = value;
                RaisePropertyChanged("RegisterClientCommand");
            }
        }

        private SubscribeCommand _subscribeCommand;
        public SubscribeCommand SubscribeCommand
        {
            get { return _subscribeCommand; }
            set
            {
                if (_subscribeCommand != null)
                {
                    
                }
                _subscribeCommand = value;
                RaisePropertyChanged("SubscribeCommand");
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

        public IViewContext ViewContext
        {
            get { return _viewContext; }
        }
    }
}