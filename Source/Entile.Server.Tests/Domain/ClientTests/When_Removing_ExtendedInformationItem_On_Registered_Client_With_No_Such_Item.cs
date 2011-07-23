using System.Collections.Generic;
using Entile.Server.Domain;
using Entile.Server.Events;
using Xunit;

namespace Entile.Server.Tests.Domain.ClientTests
{
    public class When_Removing_ExtendedInformationItem_On_Registered_Client_With_No_Such_Item : WithClient
    {
        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent("1234", "http://my.channel.com");
        }

        protected override void When(Client target)
        {
            target.RemoveExtendedInformationItem("MyKey");
        }

        [Fact]
        public void Then_No_Event_Is_Sent()
        {
            Assert.Empty(Events);
        }

        [Fact]
        public void Then_InvalidExtendedInformationItemException_Is_Thrown()
        {
            Assert.IsType<InvalidExtendedInformationItemException>(ExceptionThrown);
        }

        [Fact]
        public void Then_UniqueId_In_The_Exception_Is_Correct()
        {
            Assert.Equal("1234", ((InvalidExtendedInformationItemException)ExceptionThrown).UniqueId);
        }

        [Fact]
        public void Then_Key_In_The_Exception_Is_Correct()
        {
            Assert.Equal("MyKey", ((InvalidExtendedInformationItemException)ExceptionThrown).Key);
        }
    }
}