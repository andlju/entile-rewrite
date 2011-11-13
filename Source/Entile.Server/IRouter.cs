using System;
using System.Collections.Generic;

namespace Entile.Server
{
    public interface IRouter<THandler>
    {
        void RegisterHandler(Type messageType, THandler action);

        IEnumerable<THandler> GetHandlersFor(Type type);
    }
}