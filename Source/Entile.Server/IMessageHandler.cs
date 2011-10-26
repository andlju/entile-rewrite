using System;

namespace Entile.Server
{
    public interface IMessage
    {

    }

    public interface IMessageHandler<TMessage> where TMessage : IMessage
    {
        void Handle(TMessage command);
    }
}