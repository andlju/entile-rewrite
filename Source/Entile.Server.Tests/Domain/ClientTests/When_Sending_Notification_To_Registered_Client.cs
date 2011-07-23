using System.Collections.Generic;
using Entile.Server.Domain;
using Entile.Server.Events;
using Xunit;

namespace Entile.Server.Tests.Domain.ClientTests
{
    public class When_Successfully_Sending_Notification_To_Registered_Client : WithClient
    {
        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent("1337", "http://test.com/mychannel");
        }

        protected override void When(Client target)
        {
            target.SendNotification("Title", "Body");
        }

        [Fact]
        public void Then_No_Exception_Is_Thrown()
        {
            Assert.Null(ExceptionThrown);
        }

        [Fact]
        public void Then_Notification_Is_Sent_Once()
        {
            Assert.Equal(1, NotificationSender.NumberOfCalls);
        }

        [Fact]
        public void Then_Notification_Is_Sent_To_Registered_Channel()
        {
            Assert.Equal("http://test.com/mychannel", NotificationSender.NotificationChannel);
        }

        [Fact]
        public void Then_Sent_Notification_Title_Is_Correct()
        {
            Assert.Equal("Title", NotificationSender.Title);
        }

        [Fact]
        public void Then_Sent_Notification_Body_Is_Correct()
        {
            Assert.Equal("Body", NotificationSender.Body);
        }
    }
}