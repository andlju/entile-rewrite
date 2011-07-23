using System.Collections.Generic;
using Entile.Server.Domain;
using Entile.Server.Events;
using Xunit;

namespace Entile.Server.Tests.Domain.ClientTests
{
    public class When_Updating_Registration_On_Registered_Client : WithClient
    {
        protected 
            override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent("1234", "http://my.channel.com");
        }

        protected 
            override void When(Client client)
        {
            client.UpdateRegistration("http://new.channel.com");
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
                                                                 ev => Assert.Equal("1234", ev.UniqueId));
        }

        [Fact]
        public void Then_The_NotificationChannel_On_The_Event_Is_Correct()
        {
            AssertEvent.Contents<ClientRegistrationUpdatedEvent>(0,
                                                                 ev => Assert.Equal("http://new.channel.com", ev.NotificationChannel));
        }
    }
}