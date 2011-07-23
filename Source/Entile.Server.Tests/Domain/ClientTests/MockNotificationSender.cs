using System;
using Entile.Server.Domain;

namespace Entile.Server.Tests.Domain.ClientTests
{

    public class MockNotificationSender : INotificationSender
    {
        public Exception ExceptionToThrow { private get; set; }
        public NotificationResponse ResponseToReturn { private get; set; }
        public string NotificationChannel { get; private set; }
        public string Title { get; private set; }
        public string Body { get; private set; }

        public int NumberOfCalls { get; private set; }

        public NotificationResponse SendNotification(string channel, string title, string body)
        {
            if (ExceptionToThrow != null)
                throw ExceptionToThrow;

            NumberOfCalls++;
            NotificationChannel = channel;
            Title = title;
            Body = body;

            return ResponseToReturn;
        }
    }

}