using System;
using System.Collections.Generic;
using Entile.Server.CommandHandlers;
using Entile.Server.Commands;
using Entile.Server.Domain;
using Entile.Server.Events;
using Xunit;

namespace Entile.Server.Tests.Domain.ClientTests
{
    public class When_Successfully_Sending_Notification_To_Registered_Client : With<Client, SendToastNotificationCommand>
    {
        private readonly MockNotificationSender _notificationSender = new MockNotificationSender();

        protected override IMessageHandler<SendToastNotificationCommand> CreateHandler(IRepository<Client> repository)
        {
            return new SendNotificationCommandHandler(repository, _notificationSender, null);
        }

        protected override IEnumerable<IEvent> Given()
        {
            _notificationSender.ResponseToReturn = new NotificationResponse(200, "OK", "", "");

            yield return new ClientRegisteredEvent("1337", "http://test.com/mychannel");
        }

        protected override SendToastNotificationCommand When()
        {
            return new SendToastNotificationCommand("1234", Guid.NewGuid(), "Title", "Body", null, 3);
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
        public void Then_Notification_Succeeded_Event_Is_Pushed()
        {
            AssertEvent.IsType<NotificationSucceededEvent>(0);
        }

        [Fact]
        public void Then_Only_One_Event_Is_Pushed()
        {
            Assert.Equal(1, Events.Length);
        }

        [Fact]
        public void Then_Notification_Succeeded_Title_Is_Correct()
        {
            AssertEvent.Contents<NotificationSucceededEvent>(0, e => Assert.Equal("Title", e.Title));
        }

        [Fact]
        public void Then_Notification_Succeeded_Body_Is_Correct()
        {
            AssertEvent.Contents<NotificationSucceededEvent>(0, e => Assert.Equal("Body", e.Body));
        }

    }
}