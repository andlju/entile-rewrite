using System;
using System.Linq;
using Xunit;

namespace Entile.Server.Tests
{
    class FirstTestMessage : IMessage
    {
        public string MyProperty { get; set; }
    }
    
    class SecondTestMessage : IMessage
    {
        public int AnInt { get; set; }
    }

    class MessageHandlerExample : IMessageHandler<FirstTestMessage>, IMessageHandler<SecondTestMessage>
    {
        public FirstTestMessage FirstTestMesage;
        public SecondTestMessage SecondTestMessage;

        public void Handle(FirstTestMessage command)
        {
            FirstTestMesage = command;
        }

        public void Handle(SecondTestMessage command)
        {
            SecondTestMessage = command;
        }
    }

    public class MessageRouterTest
    {
        [Fact] 
        public void RegisteringHandlersIn_Registers_All_Handlers()
        {
            var target = new MessageRouter();
            var handler = new MessageHandlerExample();

            target.RegisterHandlersIn(handler);

            var firstHandler = target.GetHandlersFor(typeof(FirstTestMessage)).First();
            var secondHandler = target.GetHandlersFor(typeof(SecondTestMessage)).First();

            var firstTestMessage = new FirstTestMessage() {MyProperty = "TestValue"};
            var secondTestMessage = new SecondTestMessage() {AnInt = 1337};

            firstHandler.Invoke(firstTestMessage);
            secondHandler.Invoke(secondTestMessage);
            
            Assert.Equal(handler.FirstTestMesage, firstTestMessage);
            Assert.Equal(handler.SecondTestMessage, secondTestMessage);
        }
    }
}