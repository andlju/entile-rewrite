using System.Collections.Generic;
using Entile.Server.CommandHandlers;
using Entile.Server.Commands;
using Entile.Server.Domain;
using Entile.Server.Events;
using Xunit;

namespace Entile.Server.Tests.Domain.ClientTests
{
    public class When_Removing_Extended_Info_On_Registered_Client_With_Other_Extended_Info_Items : With<Client, RemoveExtendedInformationItemCommand>
    {
        protected override IMessageHandler<RemoveExtendedInformationItemCommand> CreateHandler(IRepository<Client> repository)
        {
            return new RemoveExtendedInformationItemsCommandHandler(repository);
        }

        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent("1234", "http://my.channel.com");
            yield return new ExtendedInformationItemSetEvent("MyKey", "MyValue");
            yield return new ExtendedInformationItemSetEvent("MyOtherKey", "MyOtherValue");
        }

        protected override RemoveExtendedInformationItemCommand When()
        {
            return new RemoveExtendedInformationItemCommand("1234", "MyUnknownKey");
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
            Assert.Equal("MyUnknownKey", ((InvalidExtendedInformationItemException)ExceptionThrown).Key);
        }
    }
}