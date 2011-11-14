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
    public class When_Successfully_Sending_ToastNotification_To_Subscribed_Client : With<Client, SendToastNotificationCommand>
    {
        private readonly MockNotificationSender _notificationSender = new MockNotificationSender();
        private readonly Guid _notificationId = Guid.NewGuid();
        private readonly Guid _subscriptionId = Guid.NewGuid();

        protected override IMessageHandler<SendToastNotificationCommand> CreateHandler(IRepository repository)
        {
            return new SendNotificationCommandHandler(repository, _notificationSender, null);
        }

        protected override IEnumerable<IEvent> Given()
        {
            _notificationSender.ResponseToReturn = new NotificationResponse(200, "Received", "Connected", "Active");

            yield return new ClientRegisteredEvent(UniqueId, "http://test.com/mychannel");
            yield return new SubscribedEvent(_subscriptionId, NotificationKind.Toast, "/Test.xaml?test=value", null);
        }

        protected override SendToastNotificationCommand When()
        {
            return new SendToastNotificationCommand(UniqueId, _subscriptionId, _notificationId, "Title", "Body", 3);
        }

        [TestMethod]
        public void Then_No_Exception_Is_Thrown()
        {
            Assert.IsNull(ExceptionThrown);
        }

        [TestMethod]
        public void Then_Notification_Is_Sent_Once()
        {
            Assert.AreEqual(1, _notificationSender.NumberOfCalls);
        }

        [TestMethod]
        public void Then_Notification_Is_Sent_To_Registered_Channel()
        {
            Assert.AreEqual("http://test.com/mychannel", _notificationSender.NotificationChannel);
        }

        [TestMethod]
        public void Then_Sent_Notification_Title_Is_Correct()
        {
            Assert.AreEqual("Title", ((ToastNotification)_notificationSender.NotificationMessage).Title);
        }

        [TestMethod]
        public void Then_Sent_Notification_Body_Is_Correct()
        {
            Assert.AreEqual("Body", ((ToastNotification)_notificationSender.NotificationMessage).Body);
        }

        [TestMethod]
        public void Then_Sent_Notification_ParamUri_Is_Correct()
        {
            Assert.AreEqual("/Test.xaml?test=value", ((ToastNotification)_notificationSender.NotificationMessage).ParamUri);
        }

        [TestMethod]
        public void Then_Notification_Succeeded_Event_Is_Pushed()
        {
            AssertEvent.IsType<ToastNotificationSucceededEvent>(0);
        }

        [TestMethod]
        public void Then_Only_One_Event_Is_Pushed()
        {
            Assert.AreEqual(1, Events.Length);
        }

        [TestMethod]
        public void Then_Notification_Succeeded_Title_Is_Correct()
        {
            AssertEvent.Contents<ToastNotificationSucceededEvent>(0, e => Assert.AreEqual("Title", e.Title));
        }

        [TestMethod]
        public void Then_Notification_Succeeded_Body_Is_Correct()
        {
            AssertEvent.Contents<ToastNotificationSucceededEvent>(0, e => Assert.AreEqual("Body", e.Body));
        }

        [TestMethod]
        public void Then_Notification_Succeeded_SubscriptionId_Is_Correct()
        {
            AssertEvent.Contents<ToastNotificationSucceededEvent>(0, e => Assert.AreEqual(_subscriptionId, e.SubscriptionId));
        }

        [TestMethod]
        public void Then_Notification_Succeeded_NotificationId_Is_Correct()
        {
            AssertEvent.Contents<ToastNotificationSucceededEvent>(0, e => Assert.AreEqual(_notificationId, e.NotificationId));
        }

    }
}