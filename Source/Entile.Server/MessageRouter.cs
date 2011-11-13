using System;
using System.Collections.Generic;
using System.Linq;

namespace Entile.Server
{
    public class MessageRouter<THandler> : IRouter<THandler>
    {
        private readonly IDictionary<Type, LinkedList<THandler>> _handlers;

        public MessageRouter()
        {
            _handlers = new Dictionary<Type, LinkedList<THandler>>();
        }

        public void RegisterHandler(Type messageType, THandler action)
        {
            LinkedList<THandler> handlerList;
            if (!_handlers.TryGetValue(messageType, out handlerList))
            {
                handlerList = new LinkedList<THandler>();
                _handlers[messageType] = handlerList;
            }
            handlerList.AddLast(action);
        }


        public IEnumerable<THandler> GetHandlersFor(Type type)
        {
            LinkedList<THandler> handlerList;
            if (_handlers.TryGetValue(type, out handlerList))
            {
                return handlerList.ToArray();
            }
            return new THandler[0];
        }
    }
}