using System;
using System.Collections.Generic;
using CommonDomain.Persistence;
using Entile.Server.CommandHandlers;
using Entile.Server.Commands;
using Entile.Server.Domain;
using Entile.Server.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Entile.Server.Tests.Domain.ClientTests
{
    [TestClass]
    public class When_Unsubscribing_On_Registered_Client_With_Matching_Subscription : With<Client, UnsubscribeCommand>
    {
        private Guid _clientId = Guid.NewGuid();
        private Guid _subscriptionId = Guid.NewGuid();

        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent(_clientId, "http://test.com");
            yield return
                new SubscribedEvent(_subscriptionId, NotificationKind.Tile, "/Test.xaml",
                                                new Dictionary<string, string>()
                                                    {
                                                        {"TestKey", "TestValue"},
                                                        {"InfoKey", "InfoValue"}
                                                    });
        }

        protected override IMessageHandler<UnsubscribeCommand> CreateHandler(IRepository repository)
        {
            return new UnsubscribeCommandHandler(repository);
        }

        protected override UnsubscribeCommand When()
        {
            return new UnsubscribeCommand(_clientId, _subscriptionId);
        }

        [TestMethod]
        public void Then_SubscriptionUnregisteredEvent_Is_Published()
        {
            AssertEvent.IsType<UnsbuscribedEvent>(0);
        }

        [TestMethod]
        public void Then_No_Other_Event_Is_Published()
        {
            Assert.AreEqual(1, Events.Length);
        }

        [TestMethod]
        public void Then_SubscriptionId_In_Event_Is_Correct()
        {
            AssertEvent.Contents<UnsbuscribedEvent>(0, e => Assert.AreEqual(e.SubscriptionId, _subscriptionId));
        }
    }
}