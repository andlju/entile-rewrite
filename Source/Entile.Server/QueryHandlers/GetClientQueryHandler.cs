using System.Linq;
using Entile.Server.Queries;
using Entile.Server.ViewHandlers;

namespace Entile.Server.QueryHandlers
{
    public class GetClientQueryHandler : IQueryHandler<GetClientQuery>
    {
        public object Handle(GetClientQuery query)
        {
            using (var views = new EntileViews())
            {
                var client = (from c in views.ClientViews.Include("Subscriptions.ExtendedInformation")
                              where c.ClientId == query.ClientId
                              select c).ToArray().Select(c =>
                                                         new
                                                             {
                                                                 c.ClientId,
                                                                 c.NotificationChannel,
                                                                 Subscriptions = from s in c.Subscriptions
                                                                                 select new
                                                                                            {
                                                                                                Kind =
                                                                                     s.NotificationKind,
                                                                                                s.ParamUri,
                                                                                                ExtendedInformation =
                                                                                     s.ExtendedInformation.ToDictionary(
                                                                                         i => i.Key, i => i.Value)
                                                                                            }
                                                             });

                return client.SingleOrDefault();
            }
        }
    }
}