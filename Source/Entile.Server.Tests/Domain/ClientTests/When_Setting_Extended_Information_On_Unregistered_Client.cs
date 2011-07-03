using System.Collections.Generic;
using Entile.Server.Domain;
using Entile.Server.Events;
using Xunit;

namespace Entile.Server.Tests.Domain.ClientTests
{
    public class When_Setting_Extended_Information_On_Unregistered_Client: With<Client>
    {
        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent("1234", "http://my.channel.com");
            yield return new ClientUnregisteredEvent();
        }

        protected override void When(Client target)
        {
            target.SetExtendedInformationItem("MyKey", "MyValue");
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