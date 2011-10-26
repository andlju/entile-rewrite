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
            yield return new SubscriptionRegisteredEvent(_subscriptionId, NotificationKind.Toast, "/Test.xaml?test=value", null);
        }

        protected override SendToastNotificationCommand When()
        {
            return new SendToastNotificationCommand(UniqueId, _subscriptionId, _notificationId, "Title", "Body", 3);
        }

        [Fact]
        public void Then_No_Exception_Is_Thrown()
        {
            Assert.Null(ExceptionThrown);
        }

        [Fact]
        public void Then_Notification_Is_Sent_Once()
        {
            Assert.Equal(1, _notificationSender.NumberOfCalls);
        }

        [Fact]
        public void Then_Notification_Is_Sent_To_Registered_Channel()
        {
            Assert.Equal("http://test.com/mychannel", _notificationSender.NotificationChannel);
        }

        [Fact]
        public void Then_Sent_Notification_Title_Is_Correct()
        {
            Assert.Equal("Title", ((ToastNotification)_notificationSender.NotificationMessage).Title);
        }

        [Fact]
        public void Then_Sent_Notification_Body_Is_Correct()
        {
            Assert.Equal("Body", ((ToastNotification)_notificationSender.NotificationMessage).Body);
        }

        [Fact]
        public void Then_Sent_Notification_ParamUri_Is_Correct()
        {
            Assert.Equal("/Test.xaml?test=value", ((ToastNotification)_notificationSender.NotificationMessage).ParamUri);
        }

        [Fact]
        public void Then_Notification_Succeeded_Event_Is_Pushed()
        {
            AssertEvent.IsType<ToastNotificationSucceededEvent>(0);
        }

        [Fact]
        public void Then_Only_One_Event_Is_Pushed()
        {
            Assert.Equal(1, Events.Length);
        }

        [Fact]
        public void Then_Notification_Succeeded_Title_Is_Correct()
        {
            AssertEvent.Contents<ToastNotificationSucceededEvent>(0, e => Assert.Equal("Title", e.Title));
        }

        [Fact]
        public void Then_Notification_Succeeded_Body_Is_Correct()
        {
            AssertEvent.Contents<ToastNotificationSucceededEvent>(0, e => Assert.Equal("Body", e.Body));
        }

        [Fact]
        public void Then_Notification_Succeeded_SubscriptionId_Is_Correct()
        {
            AssertEvent.Contents<ToastNotificationSucceededEvent>(0, e => Assert.Equal(_subscriptionId, e.SubscriptionId));
        }

        [Fact]
        public void Then_Notification_Succeeded_NotificationId_Is_Correct()
        {
            AssertEvent.Contents<ToastNotificationSucceededEvent>(0, e => Assert.Equal(_notificationId, e.NotificationId));
        }

    }
}