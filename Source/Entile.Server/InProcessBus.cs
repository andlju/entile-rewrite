using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Entile.Server
{

    public class MessageRouter : IMessageRouter
    {
        private readonly IDictionary<Type, LinkedList<Action<IMessage>>> _handlers;

        public MessageRouter()
        {
            _handlers = new Dictionary<Type, LinkedList<Action<IMessage>>>();
        }

        public void RegisterHandler(Type messageType, Action<IMessage> action)
        {
            LinkedList<Action<IMessage>> handlerList;
            if (!_handlers.TryGetValue(messageType, out handlerList))
            {
                handlerList = new LinkedList<Action<IMessage>>();
                _handlers[messageType] = handlerList;
            }
            handlerList.AddLast(action);
        }
        

        public IEnumerable<Action<IMessage>> GetHandlersFor(Type type)
        {
            LinkedList<Action<IMessage>> handlerList;
            if (_handlers.TryGetValue(type, out handlerList))
            {
                return handlerList.ToArray();
            }
            return new Action<IMessage>[0];
        }
    }

    public class InProcessBus : IBus
    {
        private readonly IMessageRouter _messageRouter;

        public InProcessBus(IMessageRouter messageRouter)
        {
            _messageRouter = messageRouter;
        }

        public void Publish(IMessage message)
        {
            var handlers = _messageRouter.GetHandlersFor(message.GetType());
            foreach(var handler in handlers)
            {
                handler(message);
            }
        }
    }
}