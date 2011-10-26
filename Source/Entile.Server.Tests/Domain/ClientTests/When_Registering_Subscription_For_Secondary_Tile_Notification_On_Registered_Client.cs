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
    public class When_Registering_Subscription_For_Secondary_Tile_Notification_On_Registered_Client : With<Client, RegisterSubscriptionCommand>
    {
        private Guid _clientId = Guid.NewGuid();
        private Guid _subscriptionId = Guid.NewGuid();

        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent(_clientId, "http://test.com");
        }

        protected override IMessageHandler<RegisterSubscriptionCommand> CreateHandler(IRepository repository)
        {
            return new RegisterSubscriptionCommandHandler(repository);
        }

        protected override RegisterSubscriptionCommand When()
        {
            return new RegisterSubscriptionCommand(_clientId, _subscriptionId,
                                                   NotificationKind.Tile, "/Page1.xaml?test=test",
                                                   new Dictionary<string, string>()
                                                       {
                                                           {"TestKey", "TestValue"},
                                                           {"InfoKey", "InfoValue"}
                                                       });
        }

        [Fact]
        public void Then_RegisteredForNotificationEvent_Is_Published()
        {
            AssertEvent.IsType<SubscriptionRegisteredEvent>(0);
        }

        [Fact]
        public void Then_No_Other_Event_Is_Published()
        {
            Assert.Equal(1, Events.Length);
        }

        [Fact]
        public void Then_NotificationKind_In_Event_Is_Correct()
        {
            AssertEvent.Contents<SubscriptionRegisteredEvent>(0, e => Assert.Equal(NotificationKind.Tile, e.Kind));
        }

        [Fact]
        public void Then_Uri_In_Event_Is_Correct()
        {
            AssertEvent.Contents<SubscriptionRegisteredEvent>(0, e => Assert.Equal("/Page1.xaml?test=test", e.ParamUri));
        }

        [Fact]
        public void Then_ExtendedInformation_In_Event_Is_Correct()
        {
            AssertEvent.Contents<SubscriptionRegisteredEvent>(0, e =>

                                                                 Assert.Contains(
                                                                     new KeyValuePair<string, string>("TestKey", "TestValue"),
                                                                     e.ExtendedInformation));

        }
    }
}