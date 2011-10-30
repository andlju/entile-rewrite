using System.Linq;
using Entile.Server.Events;

namespace Entile.Server.ViewHandlers
{
    public class SubscriptionViewHandler : 
        IMessageHandler<SubscribedEvent>,
        IMessageHandler<UnsbuscribedEvent>
    {
        public void Handle(SubscribedEvent command)
        {
            using (var context = new EntileViews())
            {
                var client = context.ClientViews.Include("Subscriptions.ExtendedInformation").Where(c => c.ClientId == command.AggregateId).Single();

                var sub = client.Subscriptions.Where(s => s.SubscriptionId == command.SubscriptionId).SingleOrDefault();
                if (sub == null)
                {
                    sub = new SubscriptionView();
                    client.Subscriptions.Add(sub);
                }
                else
                {
                    sub.ExtendedInformation.Clear();
                }
                sub.NotificationKind = (int) command.Kind;
                sub.ParamUri = command.ParamUri;
                
                foreach(var extendedInfo in command.ExtendedInformation)
                {
                    sub.ExtendedInformation.Add(new ExtendedInformationView() { Key = extendedInfo.Key, Value = extendedInfo.Value});
                }

                context.SaveChanges();
            }
        }

        public void Handle(UnsbuscribedEvent command)
        {
            using (var context = new EntileViews())
            {
                var client = context.ClientViews.Include("Subscriptions.ExtendedInformation").Where(c => c.ClientId == command.AggregateId).Single();

                var sub = client.Subscriptions.Where(s => s.SubscriptionId == command.SubscriptionId).SingleOrDefault();
                if (sub != null)
                {
                    client.Subscriptions.Remove(sub);
                    context.SaveChanges();
                }
            }
        }
    }
}