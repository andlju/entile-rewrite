using System.Collections.Generic;
using Entile.Server.CommandHandlers;
using Entile.Server.Commands;
using Entile.Server.Domain;
using Entile.Server.Events;
using Xunit;

namespace Entile.Server.Tests.Domain.ClientTests
{
    public class When_Setting_Extended_Info_On_Registered_Client : With<Client, SetExtendedInformationItemCommand>
    {
        protected override IMessageHandler<SetExtendedInformationItemCommand> CreateHandler(IRepository<Client> repository)
        {
            return new SetExtendedInformationItemCommandHandler(repository);
        }

        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent(UniqueId, "http://my.channel.com");
        }

        protected override SetExtendedInformationItemCommand When()
        {
            return new SetExtendedInformationItemCommand(UniqueId, "MyKey", "MyValue");
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
                                                                  ev => Assert.Equal(UniqueId, ev.UniqueId));
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