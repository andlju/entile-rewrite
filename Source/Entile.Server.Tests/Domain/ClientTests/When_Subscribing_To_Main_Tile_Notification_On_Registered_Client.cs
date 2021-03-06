﻿using System;
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
    public class When_Subscribing_To_Main_Tile_Notification_On_Registered_Client : With<Client, SubscribeCommand>
    {
        private Guid _uniqueId = Guid.NewGuid();
        private Guid _notificationId = Guid.NewGuid();

        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent(_uniqueId, "http://test.com");
        }

        protected override IMessageHandler<SubscribeCommand> CreateHandler(IRepository repository)
        {
            return new SubscribeCommandHandler(repository);
        }

        protected override SubscribeCommand When()
        {
            return new SubscribeCommand(_uniqueId, _notificationId,
                NotificationKind.Tile, string.Empty, 
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
            AssertEvent.Contents<SubscribedEvent>(0, e => Assert.AreEqual(string.Empty, e.ParamUri));
        }

        [TestMethod]
        public void Then_ExtendedInformation_In_Event_Is_Correct()
        {
            AssertEvent.Contents<SubscribedEvent>(0, e =>
                                                     Assert.IsTrue(e.ExtendedInformation.
                                                                       Any(
                                                                           ei => ei.Key == "TestKey" && ei.Value == "TestValue")));
        }
    }
}