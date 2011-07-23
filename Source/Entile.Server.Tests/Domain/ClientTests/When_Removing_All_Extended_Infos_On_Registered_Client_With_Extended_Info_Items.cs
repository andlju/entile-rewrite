using System.Collections.Generic;
using Entile.Server.CommandHandlers;
using Entile.Server.Commands;
using Entile.Server.Domain;
using Entile.Server.Events;
using Xunit;

namespace Entile.Server.Tests.Domain.ClientTests
{
    public class When_Removing_All_Extended_Infos_On_Registered_Client_With_Extended_Info_Items : With<Client, RemoveAllExtendedInformationItemsCommand>
    {
        protected override IMessageHandler<RemoveAllExtendedInformationItemsCommand> CreateHandler(IRepository<Client> repository)
        {
            return new RemoveAllExtendedInformationItemsCommandHandler(repository);
        }

        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent("1234", "http://my.channel.com");
            yield return new ExtendedInformationItemSetEvent("MyKey", "MyValue");
            yield return new ExtendedInformationItemSetEvent("MyOtherKey", "MyOtherValue");
        }

        protected override RemoveAllExtendedInformationItemsCommand When()
        {
            return new RemoveAllExtendedInformationItemsCommand("1234");
        }

        [Fact]
        public void Then_AllExtendedInformationItemsRemovedEvent_Is_Sent()
        {
            AssertEvent.IsType<AllExtendedInformationItemsRemovedEvent>(0);
        }

        [Fact]
        public void Then_UniqueId_On_Event_Is_Correct()
        {
            AssertEvent.Contents<AllExtendedInformationItemsRemovedEvent>(0,
                                                                          ev => Assert.Equal("1234", ev.UniqueId));
        }
    }
}