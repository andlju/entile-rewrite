using System;
using System.Collections.Generic;
using System.Linq;
using CommonDomain.Persistence;
using Entile.Server.CommandHandlers;
using Entile.Server.Commands;
using Entile.Server.Domain;
using Entile.Server.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Entile.Server.Tests.Domain.ClientTests
{
    [TestClass]
    public class When_Subscribing_To_Secondary_Tile_Notification_On_Registered_Client_With_Matching_Subscription : With<Client, SubscribeCommand>
    {
        private Guid _clientId = Guid.NewGuid();
        private Guid _subscriptionId = Guid.NewGuid();

        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent(_clientId, "http://test.com");
            yield return new SubscribedEvent(_subscriptionId, NotificationKind.Tile, "/Page1.xaml?test=old",
                                                         new Dictionary<string, string>()
                                                             {
                                                                 {"TestKey", "OldTestValue"},
                                                                 {"InfoKey", "OldInfoValue"}
                                                             });
        }

        protected override IMessageHandler<SubscribeCommand> CreateHandler(IRepository repository)
        {
            return new SubscribeCommandHandler(repository);
        }

        protected override SubscribeCommand When()
        {
            return new SubscribeCommand(_clientId, _subscriptionId,
                                                   NotificationKind.Tile, "/Page1.xaml?test=test",
                                                   new Dictionary<string, string>()
                                                       {
                                                           {"TestKey", "TestValue"},
                                                           {"InfoKey", "InfoValue"}
                                                       });
        }

        [TestMethod]
        public void Then_RegisteredForNotificationEvent_Is_Published()
        {
            AssertEvent.IsType<SubscribedEvent>(0);
        }

        [TestMethod]
        public void Then_No_Other_Event_Is_Published()
        {
            Assert.AreEqual(1, Events.Length);
        }

        [TestMethod]
        public void Then_NotificationKind_In_Event_Is_Correct()
        {
            AssertEvent.Contents<SubscribedEvent>(0, e => Assert.AreEqual(NotificationKind.Tile, e.Kind));
        }

        [TestMethod]
        public void Then_Uri_In_Event_Is_Correct()
        {
            AssertEvent.Contents<SubscribedEvent>(0, e => Assert.AreEqual("/Page1.xaml?test=test", e.ParamUri));
        }

        [TestMethod]
        public void Then_ExtendedInformation_In_Event_Is_Correct()
        {
            AssertEvent.Contents<SubscribedEvent>(0, e =>
                                                     Assert.IsTrue(e.ExtendedInformation.
                                                                   Any(ei => ei.Key == "TestKey" && ei.Value == "TestValue")));
        }
    }
}