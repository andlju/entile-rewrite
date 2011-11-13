using System;
using System.Linq;
using Entile.Server.QueryHandlers;

namespace Entile.Server
{
    public static class QueryRouterExtensions
    {
        public static void RegisterHandler<TMessage>(this IRouter<Func<IMessage, object>> router, Func<IMessage, object> handler)
            where TMessage : IMessage
        {
            Func<IMessage, object> action = msg => handler((TMessage)msg);

            Type messageType = typeof(TMessage);

            router.RegisterHandler(messageType, action);
        }

        public static void RegisterHandlersIn(this IRouter<Func<IMessage, object>> router, object obj)
        {
            var type = obj.GetType();

            var handlers = from i in type.GetInterfaces()
                           where
                           i.IsGenericType &&
                           i.GetGenericTypeDefinition() == typeof(IQueryHandler<>)
                           select i;

            foreach (var handler in handlers)
            {
                var messageType = handler.GetGenericArguments()[0];
                var handleMethod = handler.GetMethod("Handle");

                router.RegisterHandler(messageType, msg => handleMethod.Invoke(obj, new[] { msg }));
            }
        }
    }
}