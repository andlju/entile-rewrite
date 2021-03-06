using System.Collections.Generic;
using CommonDomain.Persistence;
using Entile.Server.CommandHandlers;
using Entile.Server.Commands;
using Entile.Server.Domain;
using Entile.Server.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Entile.Server.Tests.Domain.ClientTests
{
    [TestClass]
    public class When_Unregistering_A_Registered_Client : With<Client, UnregisterClientCommand>
    {
        protected override IMessageHandler<UnregisterClientCommand> CreateHandler(IRepository repository)
        {
            return new UnregisterClientCommandHandler(repository);
        }

        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent(UniqueId, "http://my.channel.com");
        }

        protected override UnregisterClientCommand When()
        {
            return new UnregisterClientCommand(UniqueId);
        }

        [TestMethod]
        public void Then_ClientUnregisteredEvent_Is_Sent()
        {
            AssertEvent.IsType<ClientUnregisteredEvent>(0);
        }

        [TestMethod]
        public void Then_The_UniqueId_On_The_Event_Is_Correct()
        {
            AssertEvent.Contents<ClientUnregisteredEvent>(0,
                                                          ev => Assert.AreEqual(UniqueId, ev.AggregateId));
        }
    }
}