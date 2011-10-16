using System;
using System.Collections.Generic;
using Entile.Server.CommandHandlers;
using Entile.Server.Commands;
using Entile.Server.Domain;
using Entile.Server.Events;
using Xunit;

namespace Entile.Server.Tests.Domain.ClientTests
{
    public class When_Unregistering_Subscription_On_Registered_Client_With_No_Matching_Subscription : With<Client, UnregisterSubscriptionCommand>
    {
        private Guid _clientId = Guid.NewGuid();
        private Guid _subscriptionId = Guid.NewGuid();

        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent(_clientId, "http://test.com");
            yield return
                new SubscriptionRegisteredEvent(Guid.NewGuid(), NotificationKind.Tile, "/Test.xaml",
                                                new Dictionary<string, string>()
                                                    {
                                                        {"TestKey", "TestValue"},
                                                        {"InfoKey", "InfoValue"}
                                                    });
        }

        protected override IMessageHandler<UnregisterSubscriptionCommand> CreateHandler(IRepository<Client> repository)
        {
            return new UnregisterSubscriptionCommandHandler(repository);
        }

        protected override UnregisterSubscriptionCommand When()
        {
            return new UnregisterSubscriptionCommand(_clientId, _subscriptionId);
        }

        [Fact]
        public void Then_No_Event_Is_Published()
        {
            Assert.Null(Events);
        }

        [Fact]
        public void Then_UnknowSubscriptionException_Is_Thrown()
        {
            Assert.IsType<UnknownSubscriptionException>(ExceptionThrown);
        }

        [Fact]
        public void Then_ClientId_In_Exception_Is_Correct()
        {
            Assert.Equal(_clientId, ((UnknownSubscriptionException)ExceptionThrown).ClientId);
        }

        [Fact]
        public void Then_SubscriptionId_In_Exception_Is_Correct()
        {
            Assert.Equal(_subscriptionId, ((UnknownSubscriptionException)ExceptionThrown).SubscriptionId);
        }
    }
}