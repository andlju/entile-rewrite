using System;
using System.Linq;
using Entile.Server.Domain;
using Entile.Server.Events;
using Xunit;

namespace Entile.Server.Tests.Domain.ClientTests
{
    // ReSharper disable InconsistentNaming

    public class When_Creating_New_Client : WithClient
    {
        protected override 
            Client Create()
        {
            return new Client("1234", "http://my.channel.com", null);
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
                ev => Assert.Equal("1234", ev.UniqueId)
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