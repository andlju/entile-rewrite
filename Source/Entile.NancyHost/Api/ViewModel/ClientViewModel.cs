using System;
using System.Collections.Generic;
using System.Linq;
using Entile.Server.ViewHandlers;

namespace Entile.NancyHost.Api.ViewModel
{
    public class ClientViewModel : ViewModelBase
    {
        public static ClientViewModel FromView(ClientView clientView)
        {
            if (clientView == null)
                return null;
            return new ClientViewModel(clientView);
        }

        public ClientViewModel()
        {
                
        }

        private ClientViewModel(ClientView clientView)
        {
            ClientId = clientView.ClientId;
            NotificationChannel = clientView.NotificationChannel;
            Subscriptions = clientView.Subscriptions.Select(s => SubscriptionViewModel.FromView(s)).ToList();
        }

        public Guid ClientId;

        public string NotificationChannel;

        public List<SubscriptionViewModel> Subscriptions = new List<SubscriptionViewModel>();

    }
}