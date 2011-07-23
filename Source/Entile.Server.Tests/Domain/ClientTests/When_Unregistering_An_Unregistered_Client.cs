using System.Collections.Generic;
using Entile.Server.CommandHandlers;
using Entile.Server.Commands;
using Entile.Server.Domain;
using Entile.Server.Events;
using Xunit;

namespace Entile.Server.Tests.Domain.ClientTests
{
    public class When_Unregistering_An_Unregistered_Client : With<Client, UnregisterClientCommand>
    {
        protected override IMessageHandler<UnregisterClientCommand> CreateHandler(IRepository<Client> repository)
        {
            return new UnregisterClientCommandHandler(repository);
        }

        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent("1234", "http://my.channel.com");
            yield return new ClientUnregisteredEvent();
        }

        protected override UnregisterClientCommand When()
        {
            return new UnregisterClientCommand("1234");
        }

        [Fact]
        public void Then_No_Event_Is_Sent()
        {
            Assert.Null(Events);
        }

        [Fact]
        public void Then_ClientNotRegisteredException_Should_Be_Thrown()
        {
            Assert.IsType<ClientNotRegisteredException>(ExceptionThrown);
        }

        [Fact]
        public void Then_UniqueId_In_The_Exception_Is_Correct()
        {
            Assert.Equal("1234", ((ClientNotRegisteredException)ExceptionThrown).UniqueId);
        }
    }
}