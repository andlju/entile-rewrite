using System;
using System.Linq;
using System.Reflection;

namespace Entile.Server
{

    public class InProcessMessageDispatcher : IMessageDispatcher
    {
        private readonly IRouter<Action<IMessage>> _messageRouter;

        public InProcessMessageDispatcher(IRouter<Action<IMessage>> messageRouter)
        {
            _messageRouter = messageRouter;
        }

        public void Dispatch(IMessage message)
        {
            var handlers = _messageRouter.GetHandlersFor(message.GetType());
            foreach(var handler in handlers)
            {
                handler(message);
            }
        }
    }
}