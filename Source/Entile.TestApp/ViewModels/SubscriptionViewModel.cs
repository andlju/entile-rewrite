using System;
using System.Windows.Input;
using Entile.TestApp.Actions;

namespace Entile.TestApp.ViewModels
{
    public enum SubscriptionKind
    {
        Toast,
        Tile,
        Raw
    }

    public class SubscriptionViewModel : ViewModelBase
    {
        private readonly EntileViewContext _viewContext;

        public SubscriptionViewModel(IViewContext clientViewContext, Guid subscriptionId, SubscriptionKind kind, string paramUri) 
        {
            _viewContext = new EntileViewContext(clientViewContext);
            
            _subscriptionId = subscriptionId;
            _viewContext.SetValue("SubscriptionId", _subscriptionId);

            RegisterAction("Unsubscribe", l =>
                                              {
                                                  var action = new UnsubscribeCommand(_viewContext, l.Uri);
                                                  UnsubscribeCommand = action;
                                              });
            _kind = kind;
            _paramUri = paramUri;
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