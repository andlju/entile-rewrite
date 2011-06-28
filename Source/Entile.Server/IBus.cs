using System;
using System.Collections.Generic;

namespace Entile.Server
{
    public interface IBus
    {
        void Publish(object message);
    }

    public interface IMessageRouter
    {
        void RegisterHandler<TMessage>(Action<TMessage> handler);
        IEnumerable<Action<object>> GetHandlersFor(Type type);
    }
}