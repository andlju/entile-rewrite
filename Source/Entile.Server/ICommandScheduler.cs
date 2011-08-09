using System;
using System.Collections.Generic;
using Entile.Server.Commands;

namespace Entile.Server
{
    public interface ISchedulerStore
    {
        void PushScheduledMessage(IMessage message, DateTime sendTime);
        IEnumerable<IMessage> PopMessages(int maxNumberOfMessages);
    }

    public interface IMessageScheduler
    {
        void ScheduleMessage(IMessage message, DateTime sendTime);
        IEnumerable<IMessage> GetMessagesToProcess();
    }

    public class MessageScheduler : IMessageScheduler
    {
        private readonly ISchedulerStore _schedulerStore;
        private readonly int _maximumNumberOfMessagesPerBatch = 10;

        public MessageScheduler(ISchedulerStore schedulerStore)
        {
            _schedulerStore = schedulerStore;
        }

        public void ScheduleMessage(IMessage message, DateTime sendTime)
        {
            _schedulerStore.PushScheduledMessage(message, sendTime);
        }

        public IEnumerable<IMessage> GetMessagesToProcess()
        {
            return _schedulerStore.PopMessages(_maximumNumberOfMessagesPerBatch);
        }
    }
}