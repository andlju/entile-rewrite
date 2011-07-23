using System.Collections.Generic;
using Entile.Server.Domain;
using Entile.Server.Events;
using Xunit;

namespace Entile.Server.Tests.Domain.ClientTests
{
    public class When_Setting_Extended_Info_On_Registered_Client : WithClient
    {
        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent("1234", "http://my.channel.com");
        }

        protected override void When(Client target)
        {
            target.SetExtendedInformationItem("MyKey", "MyValue");
        }

        [Fact]
        public void Then_ExtendedInformationSetEvent_Is_Sent()
        {
            AssertEvent.IsType<ExtendedInformationItemSetEvent>(0);
        }

        [Fact]
        public void Then_UniqueId_On_The_Event_Is_Correct()
        {
            AssertEvent.Contents<ExtendedInformationItemSetEvent>(0,
                                                                  ev => Assert.Equal("1234", ev.UniqueId));
        }

        [Fact]
        public void Then_Key_On_The_Event_Is_Correct()
        {
            AssertEvent.Contents<ExtendedInformationItemSetEvent>(0,
                                                                  ev => Assert.Equal("MyKey", ev.Key));
        }

        [Fact]
        public void Then_Value_On_The_Event_Is_Correct()
        {
            AssertEvent.Contents<ExtendedInformationItemSetEvent>(0,
                                                                  ev => Assert.Equal("MyValue", ev.Value));
        }
    }
}