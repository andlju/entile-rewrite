using System;

namespace Entile.Server.Commands
{

    public abstract class Notification
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

    public class SendNotificationCommand : ICommand
    {
        public string UniqueId { get; private set; }
        public Notification Notification { get; private set; }

        public SendNotificationCommand(string uniqueId, Notification notification)
        {
            UniqueId = uniqueId;
            Notification = notification;
        }
    }
}