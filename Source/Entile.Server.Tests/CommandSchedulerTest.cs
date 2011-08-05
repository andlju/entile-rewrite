using System;
using System.Collections.Generic;
using System.Linq;
using Entile.Server.Commands;
using Xunit;

namespace Entile.Server.Tests
{
    public class MockSchedulerStore : ISchedulerStore
    {
        public List<Tuple<string, DateTime>> PushedMessages = new List<Tuple<string, DateTime>>();
        public List<string> MessagesToPop = new List<string>();

        public void PushScheduledMessage(string message, DateTime sendTime)
        {
            PushedMessages.Add(new Tuple<string, DateTime>(message, sendTime));
        }

        public IEnumerable<string> PopMessages(int maxNumberOfMessages)
        {
            return MessagesToPop.Take(maxNumberOfMessages);
        }
    }

    public class TestCommand : ICommand
    {
        public long Timestamp { get; set; }

        public string Test { get; set; }
    }

    public class CommandSchedulerTest
    {
        
        [Fact]
        public void When_Scheduling_A_Command_It_Is_Pushed_To_The_Store()
        {
            var schedulerStore = new MockSchedulerStore();
            var serializer = new JsonMessageSerializer();
            
            serializer.RegisterKnownMessageType<TestCommand>();

            var commandScheduler = new CommandScheduler(schedulerStore, serializer);

            commandScheduler.ScheduleCommand(new TestCommand() { Test = "TestText"}, new DateTime(2012, 06, 24, 13, 37, 42));

            Assert.Equal(1, schedulerStore.PushedMessages.Count);
            Assert.Equal(new DateTime(2012, 06, 24, 13, 37, 42), schedulerStore.PushedMessages[0].Item2);
        }
    }
}