using System.Collections.Generic;
using System.Linq;
using Entile.Server.Queries;
using Entile.Server.ViewHandlers;

namespace Entile.Server.QueryHandlers
{
    public class SubscriptionQueries
    {
        public List<SubscriptionView> ListSubscriptions(ListSubscriptionsQuery query)
        {
            using (var views = new EntileViews())
            {
                var subs = views.ClientViews.SelectMany(c => c.Subscriptions);

                foreach (var m in query.Match)
                {
                    var m1 = m;
                    if (m1.Value != null)
                    {
                        subs = subs.Where(s => s.ExtendedInformation.Any(ei => ei.Key == m1.Key && ei.Value == m1.Value));
                    }
                    else
                    {
                        subs = subs.Where(s => s.ExtendedInformation.Any(ei => ei.Key == m1.Key));
                    }
                }

                return subs.ToList();
            }
        }
    }
}