namespace Entile.Server.Commands
{
    public class CreateNotificationCommand : CommandBase
    {
        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string Body { get; private set; }

        public CreateNotificationCommand(string uniqueId, string title, string body)
        {
            UniqueId = uniqueId;
            Title = title;
            Body = body;
        }
    }
}