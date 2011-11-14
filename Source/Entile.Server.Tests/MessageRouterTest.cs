using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Entile.Server.Tests
{
    class FirstTestMessage : IMessage
    {
        public string MyProperty { get; set; }
        public long Timestamp { get; set; }
    }
    
    class SecondTestMessage : IMessage
    {
        public int AnInt { get; set; }
        public long Timestamp { get; set; }
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

    
    [TestClass]
    public class MessageRouterTest
    {
        [TestMethod] 
        public void RegisteringHandlersIn_Registers_All_Handlers()
        {
            var target = new MessageRouter<Action<IMessage>>();
            var handler = new MessageHandlerExample();

            target.RegisterHandlersIn(handler);

            var firstHandler = target.GetHandlersFor(typeof(FirstTestMessage)).First();
            var secondHandler = target.GetHandlersFor(typeof(SecondTestMessage)).First();

            var firstTestMessage = new FirstTestMessage() {MyProperty = "TestValue"};
            var secondTestMessage = new SecondTestMessage() {AnInt = 1337};

            firstHandler.Invoke(firstTestMessage);
            secondHandler.Invoke(secondTestMessage);
            
            Assert.AreEqual(handler.FirstTestMesage, firstTestMessage);
            Assert.AreEqual(handler.SecondTestMessage, secondTestMessage);
        }
    }
}