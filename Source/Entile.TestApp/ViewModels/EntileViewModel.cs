using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Entile.TestApp.ViewModels
{
    public class EntileViewModel : ActionableObject
    {
        private readonly EntileViewContext _viewContext = new EntileViewContext();

        public EntileViewModel()
        {
            _clientId = Guid.NewGuid();
            ViewContext.SetValue("ClientId", _clientId);
            _rootApiCommand = new RootApiCommand(this, "http://localhost:4250/api");
        }

        protected override ICommand BuildCommand(string rel, string uri)
        {
            switch (rel)
            {
                case "Register":
                    return RegisterClientCommand = new RegisterClientCommand(this, uri);
            }
            return null;
        }

        protected override void RemoveCommand(string rel)
        {
            switch (rel)
            {
                case "Register":
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
}