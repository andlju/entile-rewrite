using System;
using System.Collections.Generic;
using System.Linq;
using Entile.Server.ViewHandlers;

namespace Entile.NancyHost.Api.ViewModel
{
    public class SubscriptionViewModel : ViewModelBase
    {
        public static SubscriptionViewModel FromView(SubscriptionView subscriptionView)
        {
            if (subscriptionView == null)
                return null;

            return new SubscriptionViewModel(subscriptionView);
        }

        public SubscriptionViewModel()
        {
                
        }

        private SubscriptionViewModel(SubscriptionView subscriptionView)
        {
            ClientId = subscriptionView.ClientId;
            SubscriptionId = subscriptionView.SubscriptionId;
            NotificationKind = subscriptionView.NotificationKind;
            ParamUri = subscriptionView.ParamUri;
            ExtendedInformation = subscriptionView.ExtendedInformation.ToDictionary(e => e.Key, e => e.Value);
        }

        public Guid ClientId;

        public Guid SubscriptionId;

        public int NotificationKind;
        public string ParamUri;

        public Dictionary<string, string> ExtendedInformation = new Dictionary<string, string>();
    }
}