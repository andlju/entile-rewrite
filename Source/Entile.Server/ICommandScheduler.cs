using System;
using System.Collections.Generic;
using Entile.Server.Commands;

namespace Entile.Server
{
    public interface ISchedulerStore
    {
        void PushScheduledMessage(string message, DateTime sendTime);
        IEnumerable<string> PopMessages(int maxNumberOfMessages);
    }

    public interface ICommandScheduler
    {
        void ScheduleCommand(ICommand command, DateTime sendTime);
        IEnumerable<ICommand> GetMessagesToProcess();
    }

    public class CommandScheduler : ICommandScheduler
    {
        private readonly ISchedulerStore _schedulerStore;
        private readonly IMessageSerializer _serializer;
        private readonly int _maximumNumberOfMessagesPerBatch = 10;

        public CommandScheduler(ISchedulerStore schedulerStore, IMessageSerializer serializer)
        {
            _schedulerStore = schedulerStore;
            _serializer = serializer;
        }

        public void ScheduleCommand(ICommand command, DateTime sendTime)
        {
            var message = _serializer.Serialize(command);
            _schedulerStore.PushScheduledMessage(message, sendTime);
        }

        public IEnumerable<ICommand> GetMessagesToProcess()
        {
            var messages = _schedulerStore.PopMessages(_maximumNumberOfMessagesPerBatch);
            foreach(var message in messages)
            {
                yield return (ICommand)_serializer.Deserialize(message);
            }
        }
    }
}