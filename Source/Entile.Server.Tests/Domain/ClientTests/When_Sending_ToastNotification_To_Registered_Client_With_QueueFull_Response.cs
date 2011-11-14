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
    public class When_Sending_ToastNotification_To_Subscribed_Client_With_QueueFull_Response : With<Client, SendToastNotificationCommand>
    {
        private readonly MockNotificationSender _notificationSender = new MockNotificationSender();
        private readonly MockMessageScheduler _messageScheduler = new MockMessageScheduler();

        private Guid _notificationId = Guid.NewGuid();
        private Guid _subscriptionId = Guid.NewGuid();

        protected override IMessageHandler<SendToastNotificationCommand> CreateHandler(IRepository repository)
        {
            return new SendNotificationCommandHandler(repository, _notificationSender, _messageScheduler);
        }

        protected override IEnumerable<IEvent> Given()
        {
            _notificationSender.ResponseToReturn = new NotificationResponse(200, "QueueFull", "Connected", "Active");

            yield return new ClientRegisteredEvent(UniqueId, "http://test.com/mychannel");
            yield return new SubscribedEvent(_subscriptionId, NotificationKind.Toast, "/Test.xaml?test=value", null);
        }

        protected override SendToastNotificationCommand When()
        {
            return new SendToastNotificationCommand(UniqueId, _subscriptionId, _notificationId, "Title", "Body", 3);
        }

        [TestMethod]
        public void Then_Notification_Failed_Event_Is_Sent()
        {
            AssertEvent.IsType<ToastNotificationFailedEvent>(0);
        }

        [TestMethod]
        public void Then_Failed_Notification_Title_Is_Correct()
        {
            AssertEvent.Contents<ToastNotificationFailedEvent>(0, e => Assert.AreEqual("Title", e.Title));
        }

        [TestMethod]
        public void Then_Failed_Notification_Body_Is_Correct()
        {
            AssertEvent.Contents<ToastNotificationFailedEvent>(0, e => Assert.AreEqual("Body", e.Body));
        }

        [TestMethod]
        public void Then_One_Command_Is_Scheduled()
        {
            Assert.AreEqual(1, _messageScheduler.ScheduledMessages.Count);
        }

        [TestMethod]
        public void Then_SendToastNotificationCommand_Is_Scheduled()
        {
            Assert.IsInstanceOfType(_messageScheduler.ScheduledMessages[0].Item1, typeof(SendToastNotificationCommand));
        }

        [TestMethod]
        public void Then_ScheduledCommand_Has_Correct_Title()
        {
            Assert.AreEqual("Title", _messageScheduler.ScheduledMessage<SendToastNotificationCommand>(0).Title);
        }

        [TestMethod]
        public void Then_ScheduledCommand_Has_Correct_Body()
        {
            Assert.AreEqual("Body", _messageScheduler.ScheduledMessage<SendToastNotificationCommand>(0).Body);
        }

        [TestMethod]
        public void Then_ScheduledCommand_Has_Correct_SubscriptionId()
        {
            Assert.AreEqual(_subscriptionId, _messageScheduler.ScheduledMessage<SendToastNotificationCommand>(0).SubscriptionId);
        }

        [TestMethod]
        public void Then_ScheduledCommand_Has_Correct_NotificationId()
        {
            Assert.AreEqual(_notificationId, _messageScheduler.ScheduledMessage<SendToastNotificationCommand>(0).NotificationId);
        }
    }
}