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
    public class When_Unregistering_An_Unregistered_Client : With<Client, UnregisterClientCommand>
    {
        protected override IMessageHandler<UnregisterClientCommand> CreateHandler(IRepository repository)
        {
            return new UnregisterClientCommandHandler(repository);
        }

        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent(UniqueId, "http://my.channel.com");
            yield return new ClientUnregisteredEvent();
        }

        protected override UnregisterClientCommand When()
        {
            return new UnregisterClientCommand(UniqueId);
        }

        [TestMethod]
        public void Then_No_Event_Is_Sent()
        {
            Assert.IsNull(Events);
        }

        [TestMethod]
        public void Then_ClientNotRegisteredException_Should_Be_Thrown()
        {
            Assert.IsInstanceOfType(ExceptionThrown, typeof(ClientNotRegisteredException));
        }

        [TestMethod]
        public void Then_UniqueId_In_The_Exception_Is_Correct()
        {
            Assert.AreEqual(UniqueId, ((ClientNotRegisteredException)ExceptionThrown).ClientId);
        }
    }
}