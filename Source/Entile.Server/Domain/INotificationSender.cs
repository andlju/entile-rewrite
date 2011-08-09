namespace Entile.Server.Domain
{

    public interface INotificationSender
    {
        NotificationResponse SendNotification(string channel, INotificationMessage notification);
    }

    public class DummyNotificationSender : INotificationSender
    {
        public NotificationResponse SendNotification(string channel, INotificationMessage notification)
        {
            return new NotificationResponse(200, "Received", "Connected", "Active");
        }
    }


}