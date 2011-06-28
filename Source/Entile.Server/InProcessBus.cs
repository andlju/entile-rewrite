using System;
using System.Collections.Generic;
using System.Linq;

namespace Entile.Server
{
    public class MessageRouter : IMessageRouter
    {
        private readonly IDictionary<Type, LinkedList<Action<object>>> _handlers;

        public MessageRouter()
        {
            _handlers = new Dictionary<Type, LinkedList<Action<object>>>();
        }

        public void RegisterHandler<TMessage>(Action<TMessage> handler)
        {
            LinkedList<Action<object>> handlerList;
            if (!_handlers.TryGetValue(typeof(TMessage), out handlerList))
            {
                handlerList = new LinkedList<Action<object>>();
                _handlers[typeof(TMessage)] = handlerList;
            }
            handlerList.AddLast(msg => handler((TMessage)msg));
        }

        public IEnumerable<Action<object>> GetHandlersFor(Type type)
        {
            LinkedList<Action<object>> handlerList;
            if (_handlers.TryGetValue(type, out handlerList))
            {
                return handlerList.ToArray();
            }
            return new Action<object>[0];
        }
    }

    public class InProcessBus : IBus
    {
        private readonly IMessageRouter _messageRouter;

        public InProcessBus(IMessageRouter messageRouter)
        {
            _messageRouter = messageRouter;
        }

        public void Publish(object message)
        {
            var handlers = _messageRouter.GetHandlersFor(message.GetType());
            foreach(var handler in handlers)
            {
                handler(message);
            }
        }
    }
}