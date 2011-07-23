namespace Entile.Server.Events
{
    public class NotificationSucceededEvent : EventBase
    {
        public string Title { get; private set; }
        public string Body { get; private set; }

        public NotificationSucceededEvent(string title, string body)
        {
            Title = title;
            Body = body;
        }
    }
}