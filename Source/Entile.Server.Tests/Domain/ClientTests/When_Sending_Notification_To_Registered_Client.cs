using System;
using System.Collections.Generic;
using Entile.Server.Domain;
using Entile.Server.Events;
using Xunit;

namespace Entile.Server.Tests.Domain.ClientTests
{
    public class When_Successfully_Sending_Notification_To_Registered_Client : With<Client, SendNotificationCommand>
    {
        private readonly MockNotificationSender _notificationSender = new MockNotificationSender();

        protected override IMessageHandler<SendNotificationCommand> CreateHandler(IRepository<Client> repository)
        {
            throw new NotImplementedException();

            // return new SendNotificationComandHandler(repository);
        }
        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent("1337", "http://test.com/mychannel");
        }

        protected override SendNotificationCommand When()
        {
            throw new NotImplementedException();
            // return new SendNotificationCommand("1234", "Title", "Body");
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
    }
}