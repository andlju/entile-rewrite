namespace Entile.Server.Events
{
    public class NotificationFailedEvent : EventBase
    {
        public string Title { get; private set; }
        public string Body { get; private set; }

        public NotificationFailedEvent(string title, string body)
        {
            Title = title;
            Body = body;
        }
    }
}