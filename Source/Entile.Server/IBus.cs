using System;
using System.Collections.Generic;
using System.Linq;

namespace Entile.Server
{
    public interface IBus
    {
        void Publish(IMessage message);
    }

    public static class MessageRouterExtensions
    {
        public static void RegisterHandler<TMessage>(this IMessageRouter router, Action<TMessage> handler)
            where TMessage : IMessage
        {
            Action<IMessage> action = msg => handler((TMessage)msg);

            Type messageType = typeof(TMessage);

            router.RegisterHandler(messageType, action);
        }

        public static void RegisterHandlersIn(this IMessageRouter router, object obj)
        {
            var type = obj.GetType();

            var handlers = from i in type.GetInterfaces()
                           where
                           i.IsGenericType &&
                           i.GetGenericTypeDefinition() == typeof(IMessageHandler<>)
                           select i;

            foreach (var handler in handlers)
            {
                var messageType = handler.GetGenericArguments()[0];
                var handleMethod = handler.GetMethod("Handle");

                router.RegisterHandler(messageType, msg => handleMethod.Invoke(obj, new[] { msg }));
            }
        }

    }

    public interface IMessageRouter
    {
        void RegisterHandler(Type messageType, Action<IMessage> action);

        IEnumerable<Action<IMessage>> GetHandlersFor(Type type);
    }
}