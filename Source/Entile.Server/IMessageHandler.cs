using System;

namespace Entile.Server
{
    public interface IMessage
    {
        long Timestamp { get; set; }
    }

    public interface IMessageHandler<TMessage> where TMessage : IMessage
    {
        void Handle(TMessage command);
    }
}