using System;

namespace Entile.Server.Commands
{
    public class SendRawNotificationCommand : CommandBase
    {
        public string UniqueId { get; private set; }
        public Guid NotificationId { get; private set; }
        public string Content { get; private set; }
        public int NumberOfAttempts { get; private set; }

        public SendRawNotificationCommand(string uniqueId, Guid notificationId, string content, int numberOfAttempts)
        {
            UniqueId = uniqueId;
            NotificationId = notificationId;
            Content = content;
            NumberOfAttempts = numberOfAttempts;
        }
    }
}