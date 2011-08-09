using System;

namespace Entile.Server.Commands
{
    public class SendToastNotificationCommand : CommandBase
    {
        public string UniqueId { get; private set; }
        public Guid NotificationId { get; private set; }
        public string Title { get; private set; }
        public string Body { get; private set; }
        public string ParamUri { get; private set; }
        public int NumberOfAttempts { get; private set; }

        public SendToastNotificationCommand(string uniqueId, Guid notificationId, string title, string body, string paramUri, int numberOfAttempts)
        {
            UniqueId = uniqueId;
            NotificationId = notificationId;
            Title = title;
            Body = body;
            ParamUri = paramUri;
            NumberOfAttempts = numberOfAttempts;
        }
    }
}