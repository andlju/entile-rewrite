using System;
using System.Collections.Generic;

namespace Entile.Server.Tests.Domain.ClientTests
{
    public class MockMessageScheduler : IMessageScheduler
    {
        public List<Tuple<IMessage, DateTime>> ScheduledMessages = new List<Tuple<IMessage, DateTime>>();

        public TMessage ScheduledMessage<TMessage>(int messageNumber)
        {
            return (TMessage)ScheduledMessages[messageNumber].Item1;
        }

        public void ScheduleMessage(IMessage message, DateTime sendTime)
        {
            ScheduledMessages.Add(new Tuple<IMessage, DateTime>(message, sendTime));
        }

        public IEnumerable<IMessage> GetMessagesToProcess()
        {
            throw new NotImplementedException();
        }
    }
}