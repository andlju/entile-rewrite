using System;

namespace Entile.Server.Commands
{
    public class SendToastNotificationCommand : CommandBase
    {
        public readonly Guid ClientId;
        public readonly Guid NotificationId;
        public readonly string Title;
        public readonly string Body;
        public readonly string ParamUri;
        public readonly int NumberOfAttempts;

        public SendToastNotificationCommand(Guid clientId, Guid notificationId, string title, string body, string paramUri, int numberOfAttempts)
        {
            ClientId = clientId;
            NotificationId = notificationId;
            Title = title;
            Body = body;
            ParamUri = paramUri;
            NumberOfAttempts = numberOfAttempts;
        }
    }
}