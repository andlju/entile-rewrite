using System;
using System.Linq;
using Entile.Server.Queries;
using Entile.Server.ViewHandlers;

namespace Entile.Server.QueryHandlers
{

    public class ClientQueries
    {
        public ClientView GetClient(GetClientQuery query)
        {
            using (var views = new EntileViews())
            {
                var client = (from c in views.ClientViews.Include("Subscriptions.ExtendedInformation")
                              where c.ClientId == query.ClientId
                              select c);

                return client.SingleOrDefault();
            }
        }

        public SubscriptionView GetSubscription(GetSubscriptionQuery query)
        {
            using (var views = new EntileViews())
            {
                var client = (from c in views.ClientViews.Include("Subscriptions.ExtendedInformation")
                              where c.ClientId == query.ClientId
                              select c).SingleOrDefault();
                
                if (client == null)
                    return null;

                return client.Subscriptions.Where(s => s.SubscriptionId == query.SubscriptionId).SingleOrDefault();
            }
        }
    }
}