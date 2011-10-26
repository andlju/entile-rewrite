using System;
using System.Collections.Generic;
using CommonDomain.Persistence;
using Entile.Server.CommandHandlers;
using Entile.Server.Commands;
using Entile.Server.Domain;
using Entile.Server.Events;
using Xunit;

namespace Entile.Server.Tests.Domain.ClientTests
{
    public class When_Unregistering_Subscription_On_Registered_Client_With_Matching_Subscription : With<Client, UnregisterSubscriptionCommand>
    {
        private Guid _clientId = Guid.NewGuid();
        private Guid _subscriptionId = Guid.NewGuid();

        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent(_clientId, "http://test.com");
            yield return
                new SubscriptionRegisteredEvent(_subscriptionId, NotificationKind.Tile, "/Test.xaml",
                                                new Dictionary<string, string>()
                                                    {
                                                        {"TestKey", "TestValue"},
                                                        {"InfoKey", "InfoValue"}
                                                    });
        }

        protected override IMessageHandler<UnregisterSubscriptionCommand> CreateHandler(IRepository repository)
        {
            return new UnregisterSubscriptionCommandHandler(repository);
        }

        protected override UnregisterSubscriptionCommand When()
        {
            return new UnregisterSubscriptionCommand(_clientId, _subscriptionId);
        }

        [Fact]
        public void Then_SubscriptionUnregisteredEvent_Is_Published()
        {
            AssertEvent.IsType<SubscriptionUnregisteredEvent>(0);
        }

        [Fact]
        public void Then_No_Other_Event_Is_Published()
        {
            Assert.Equal(1, Events.Length);
        }

        [Fact]
        public void Then_SubscriptionId_In_Event_Is_Correct()
        {
            AssertEvent.Contents<SubscriptionUnregisteredEvent>(0, e => Assert.Equal(e.SubscriptionId, _subscriptionId));
        }
    }
}