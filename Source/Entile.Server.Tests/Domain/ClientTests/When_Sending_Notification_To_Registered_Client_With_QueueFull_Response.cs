using System;
using System.Collections.Generic;
using Entile.Server.CommandHandlers;
using Entile.Server.Commands;
using Entile.Server.Domain;
using Entile.Server.Events;
using Xunit;

namespace Entile.Server.Tests.Domain.ClientTests
{
    public class When_Sending_Notification_To_Registered_Client_With_QueueFull_Response : With<Client, SendToastNotificationCommand>
    {
        private readonly MockNotificationSender _notificationSender = new MockNotificationSender();

        protected override IMessageHandler<SendToastNotificationCommand> CreateHandler(IRepository<Client> repository)
        {
            return new SendNotificationCommandHandler(repository, _notificationSender, null);
        }

        protected override IEnumerable<IEvent> Given()
        {
            _notificationSender.ResponseToReturn = new NotificationResponse(200, "QueueFull", "", "");

            yield return new ClientRegisteredEvent("1337", "http://test.com/mychannel");
        }

        protected override SendToastNotificationCommand When()
        {
            return new SendToastNotificationCommand("1234", Guid.NewGuid(), "Title", "Body", null, 3);
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
}