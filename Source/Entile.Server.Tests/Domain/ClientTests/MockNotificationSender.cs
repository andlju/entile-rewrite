using System;
using Entile.Server.Domain;

namespace Entile.Server.Tests.Domain.ClientTests
{

    public class MockNotificationSender : INotificationSender
    {
        public Exception ExceptionToThrow { private get; set; }
        public NotificationResponse ResponseToReturn { private get; set; }
        public string NotificationChannel { get; private set; }
        public INotificationMessage NotificationMessage { get; private set; }

        public int NumberOfCalls { get; private set; }

        public NotificationResponse SendNotification(string channel, INotificationMessage notification)
        {
            if (ExceptionToThrow != null)
                throw ExceptionToThrow;

            NumberOfCalls++;
            NotificationChannel = channel;
            NotificationMessage = notification;

            return ResponseToReturn;
        }
    }

}