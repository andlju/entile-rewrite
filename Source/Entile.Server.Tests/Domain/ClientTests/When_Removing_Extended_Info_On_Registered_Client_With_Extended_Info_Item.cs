using System.Collections.Generic;
using Entile.Server.CommandHandlers;
using Entile.Server.Commands;
using Entile.Server.Domain;
using Entile.Server.Events;
using Xunit;

namespace Entile.Server.Tests.Domain.ClientTests
{
    public class When_Removing_Extended_Info_On_Registered_Client_With_Extended_Info_Item : With<Client, RemoveExtendedInformationItemCommand>
    {
        protected override IMessageHandler<RemoveExtendedInformationItemCommand> CreateHandler(IRepository<Client> repository)
        {
            return new RemoveExtendedInformationItemsCommandHandler(repository);
        }

        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent(UniqueId, "http://my.channel.com");
            yield return new ExtendedInformationItemSetEvent("MyKey", "MyValue");
        }

        protected override RemoveExtendedInformationItemCommand When()
        {
            return new RemoveExtendedInformationItemCommand(UniqueId, "MyKey");
        }

        [Fact]
        public void Then_ExtendedInformationItemRemovedEvent_Is_Sent()
        {
            AssertEvent.IsType<ExtendedInformationItemRemovedEvent>(0);
        }

        [Fact]
        public void Then_UniqueId_On_Event_Is_Correct()
        {
            AssertEvent.Contents<ExtendedInformationItemRemovedEvent>(0,
                                                                      ev => Assert.Equal(UniqueId, ev.AggregateId));
        }

        [Fact]
        public void Then_Key_On_Event_Is_Correct()
        {
            AssertEvent.Contents<ExtendedInformationItemRemovedEvent>(0, 
                                                                      ev => Assert.Equal("MyKey", ev.Key));
        }
    }
}