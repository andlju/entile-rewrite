using System;
using System.Collections.Generic;
using Entile.Server.CommandHandlers;
using Entile.Server.Commands;
using Entile.Server.Domain;
using Entile.Server.Events;
using Xunit;

namespace Entile.Server.Tests.Domain.ClientTests
{
    public class When_Sending_Notification_To_Registered_Client_With_QueueFull_Response : With<Client, SendNotificationCommand>
    {
        private readonly MockNotificationSender _notificationSender = new MockNotificationSender();

        protected override IMessageHandler<SendNotificationCommand> CreateHandler(IRepository<Client> repository)
        {
            return new SendNotificationCommandHandler(repository, _notificationSender, null);
        }

        protected override IEnumerable<IEvent> Given()
        {
            _notificationSender.ResponseToReturn = new NotificationResponse("QueueFull");

            yield return new ClientRegisteredEvent("1337", "http://test.com/mychannel");
        }

        protected override SendNotificationCommand When()
        {
            return new SendNotificationCommand("1337", "Title", "Body");
        }

        [Fact]
        public void Then_Notification_Failed_Event_Is_Sent()
        {
            AssertEvent.IsType<NotificationFailedEvent>(0);
        }

        [Fact]
        public void Then_Failed_Notification_Title_Is_Correct()
        {
            AssertEvent.Contents<NotificationFailedEvent>(0, e => Assert.Equal("Title", e.Title));
        }

        [Fact]
        public void Then_Failed_Notification_Body_Is_Correct()
        {
            AssertEvent.Contents<NotificationFailedEvent>(0, e => Assert.Equal("Body", e.Body));
        }
    }


    public class When_Successfully_Sending_Notification_To_Registered_Client : With<Client, SendNotificationCommand>
    {
        private readonly MockNotificationSender _notificationSender = new MockNotificationSender();

        protected override IMessageHandler<SendNotificationCommand> CreateHandler(IRepository<Client> repository)
        {
            return new SendNotificationCommandHandler(repository, _notificationSender, null);
        }

        protected override IEnumerable<IEvent> Given()
        {
            _notificationSender.ResponseToReturn = new NotificationResponse("Sent");

            yield return new ClientRegisteredEvent("1337", "http://test.com/mychannel");
        }

        protected override SendNotificationCommand When()
        {
            return new SendNotificationCommand("1234", "Title", "Body");
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
            Assert.Equal("Title", _notificationSender.Title);
        }

        [Fact]
        public void Then_Sent_Notification_Body_Is_Correct()
        {
            Assert.Equal("Body", _notificationSender.Body);
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