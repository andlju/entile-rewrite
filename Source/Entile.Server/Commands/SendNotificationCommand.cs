using System;

namespace Entile.Server.Commands
{

/*    public abstract class Notification
    {
        protected Notification()
        {
            NotificationId = Guid.NewGuid();
        }

        public Guid NotificationId { get; set; }
    }

    public class ToastNotification : Notification
    {
        public string Title { get; set; }
        public string Body { get; set; }
    }
    */
    public class SendNotificationCommand : CommandBase
    {
        public string UniqueId { get; private set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public SendNotificationCommand(string uniqueId, string title, string body)
        {
            UniqueId = uniqueId;
            Title = title;
            Body = body;
        }
    }
}