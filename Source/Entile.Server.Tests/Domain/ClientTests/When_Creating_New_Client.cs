using System;
using System.Linq;
using CommonDomain.Persistence;
using Entile.Server.CommandHandlers;
using Entile.Server.Commands;
using Entile.Server.Domain;
using Entile.Server.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Entile.Server.Tests.Domain.ClientTests
{
    // ReSharper disable InconsistentNaming
    [TestClass]
    public class When_Registering_New_Client : With<Client, RegisterClientCommand>
    {
        protected override IMessageHandler<RegisterClientCommand> CreateHandler(IRepository repository)
        {
            return new RegisterClientCommandHandler(repository);
        }

        protected override RegisterClientCommand When()
        {
            return new RegisterClientCommand(UniqueId, "http://my.channel.com");
        }

        [TestMethod]
        public void Then_ClientRegisteredEvent_Is_Sent()
        {
            AssertEvent.IsType<ClientRegisteredEvent>(0);
        }

        [TestMethod]
        public void Then_The_UniqueId_On_The_Event_Is_Correct()
        {
            AssertEvent.Contents<ClientRegisteredEvent>(0,
                ev => Assert.AreEqual(UniqueId, ev.AggregateId)
                );
        }

        [TestMethod]
        public void Then_The_NotificationChannel_On_The_Event_Is_Correct()
        {
            AssertEvent.Contents<ClientRegisteredEvent>(0, 
                ev => Assert.AreEqual("http://my.channel.com", ev.NotificationChannel)
                );
        }
    }

    // ReSharper restore InconsistentNaming
}