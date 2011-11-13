using System.Collections.ObjectModel;
using System.Linq;
using Entile.Server.Events;

namespace Entile.Server.ViewHandlers
{
    public class SubscriptionViewHandler : 
        IMessageHandler<SubscribedEvent>,
        IMessageHandler<UnsbuscribedEvent>
    {
        public void Handle(SubscribedEvent evt)
        {
            using (var context = new EntileViews())
            {
                var client = context.ClientViews.Include("Subscriptions.ExtendedInformation").Where(c => c.ClientId == evt.AggregateId).Single();

                var sub = client.Subscriptions.Where(s => s.SubscriptionId == evt.SubscriptionId).SingleOrDefault();
                if (sub == null)
                {
                    sub = new SubscriptionView
                              {
                                  SubscriptionId = evt.SubscriptionId,
                                  ExtendedInformation = new Collection<ExtendedInformationView>()
                              };
                    client.Subscriptions.Add(sub);
                }
                else
                {
                    sub.ExtendedInformation.Clear();
                }
                sub.NotificationKind = (int) evt.Kind;
                sub.ParamUri = evt.ParamUri;

                if (evt.ExtendedInformation != null)
                {
                    foreach (var extendedInfo in evt.ExtendedInformation)
                    {
                        sub.ExtendedInformation.Add(new ExtendedInformationView()
                                                        {Key = extendedInfo.Key, Value = extendedInfo.Value});
                    }
                }

                context.SaveChanges();
            }
        }

        public void Handle(UnsbuscribedEvent evt)
        {
            using (var context = new EntileViews())
            {
                var client = context.ClientViews.Include("Subscriptions.ExtendedInformation").Where(c => c.ClientId == evt.AggregateId).Single();

                var sub = client.Subscriptions.Where(s => s.SubscriptionId == evt.SubscriptionId).SingleOrDefault();
                if (sub != null)
                {
                    client.Subscriptions.Remove(sub);
                    context.SaveChanges();
                }
            }
        }
    }
}