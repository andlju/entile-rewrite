using System;
using System.Linq;

namespace Entile.Server
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IRouter<Func<IMessage, object>> _queryRouter;

        public QueryDispatcher(IRouter<Func<IMessage, object>> queryRouter)
        {
            _queryRouter = queryRouter;
        }

        public object Invoke(IMessage query)
        {
            var handler = _queryRouter.GetHandlersFor(query.GetType()).SingleOrDefault();

            if (handler == null)
                throw new InvalidOperationException(string.Format("No QueryHandler registered for {0}", query.GetType()));

            return handler.Invoke(query);
        }
    }
}