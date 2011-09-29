using System.Collections.Generic;
using Entile.Server.CommandHandlers;
using Entile.Server.Commands;
using Entile.Server.Domain;
using Entile.Server.Events;
using Xunit;

namespace Entile.Server.Tests.Domain.ClientTests
{
    public class When_Registering_Already_Registered_Client : With<Client, RegisterClientCommand>
    {
        protected override IMessageHandler<RegisterClientCommand> CreateHandler(IRepository<Client> repository)
        {
            return new RegisterClientCommandHandler(repository);
        }

        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent(UniqueId, "http://my.channel.com");
        }

        protected override RegisterClientCommand When()
        {
            return new RegisterClientCommand(UniqueId, "http://new.channel.com");
        }

        [Fact]
        public void Then_ClientRegistrationUpdatedEvent_Is_Sent()
        {
            AssertEvent.IsType<ClientRegistrationUpdatedEvent>(0);
        }

        [Fact]
        public void Then_The_UniqueId_On_The_Event_Is_Correct()
        {
            AssertEvent.Contents<ClientRegistrationUpdatedEvent>(0,
                                                                 ev => Assert.Equal(UniqueId, ev.UniqueId));
        }

        [Fact]
        public void Then_The_NotificationChannel_On_The_Event_Is_Correct()
        {
            AssertEvent.Contents<ClientRegistrationUpdatedEvent>(0,
                                                                 ev => Assert.Equal("http://new.channel.com", ev.NotificationChannel));
        }
    }
}