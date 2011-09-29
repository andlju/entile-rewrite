using System;
using System.Linq;
using Entile.Server.CommandHandlers;
using Entile.Server.Commands;
using Entile.Server.Domain;
using Entile.Server.Events;
using Xunit;

namespace Entile.Server.Tests.Domain.ClientTests
{
    // ReSharper disable InconsistentNaming

    public class When_Registering_New_Client : With<Client, RegisterClientCommand>
    {
        protected override IMessageHandler<RegisterClientCommand> CreateHandler(IRepository<Client> repository)
        {
            return new RegisterClientCommandHandler(repository);
        }

        protected override System.Collections.Generic.IEnumerable<IEvent> Given()
        {
            return null;
        }

        protected override RegisterClientCommand When()
        {
            return new RegisterClientCommand(UniqueId, "http://my.channel.com");
        }

        [Fact]
        public void Then_ClientRegisteredEvent_Is_Sent()
        { 
            AssertEvent.IsType<ClientRegisteredEvent>(0);
        }

        [Fact]
        public void Then_The_UniqueId_On_The_Event_Is_Correct()
        {
            AssertEvent.Contents<ClientRegisteredEvent>(0,
                ev => Assert.Equal(UniqueId, ev.UniqueId)
                );
        }

        [Fact]
        public void Then_The_NotificationChannel_On_The_Event_Is_Correct()
        {
            AssertEvent.Contents<ClientRegisteredEvent>(0, 
                ev => Assert.Equal("http://my.channel.com", ev.NotificationChannel)
                );
        }
    }

    // ReSharper restore InconsistentNaming
}