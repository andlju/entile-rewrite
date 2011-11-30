using System;
using System.Windows.Input;

namespace Entile.TestApp.ViewModels
{
    public enum SubscriptionKind
    {
        Toast,
        Tile,
        Raw
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

        protected override ICommand BuildCommand(string rel, string uri)
        {
            switch(rel)
            {
                case "Unsubscribe":
                    return UnsubscribeCommand = new UnsubscribeCommand(this, uri);
            }
            return null;
        }

        protected override void RemoveCommand(string rel)
        {
            switch (rel)
            {
                case "Unsubscribe":
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
}