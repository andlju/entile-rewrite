using System.Dynamic;
using System.Linq;
using Entile.Server.Queries;
using Entile.Server.ViewHandlers;

namespace Entile.Server.QueryHandlers
{
    public class GetClientQueryHandler : IQueryHandler<GetClientQuery>
    {
        public dynamic Handle(GetClientQuery query)
        {
            using (var views = new EntileViews())
            {
                var client = (from c in views.ClientViews.Include("Subscriptions.ExtendedInformation")
                              where c.ClientId == query.ClientId
                              select c).ToArray().Select(BuildClient);
                
                return client.SingleOrDefault();
            }
        }

        private static dynamic BuildClient(ClientView c)
        {
            dynamic cl = new ExpandoObject();
            cl.ClientId = c.ClientId;
            cl.NotificationChannel = c.NotificationChannel;
            cl.Subscriptions = c.Subscriptions.Select(BuildSubscription).ToArray();
            return cl;
        }

        private static dynamic BuildSubscription(SubscriptionView s)
        {
            dynamic su = new ExpandoObject();
            su.SubscriptionId = s.SubscriptionId;
            su.Kind = s.NotificationKind;
            su.ParamUri = s.ParamUri;
            su.ExtendedInformation =
                s.ExtendedInformation.ToDictionary(i => i.Key, i => i.Value);
            return su;
        }
    }
}