using System;

namespace Entile.Server.Commands
{
    public class CreateNotificationCommand : CommandBase
    {
        public Guid UniqueId { get; private set; }
        public string Title { get; private set; }
        public string Body { get; private set; }

        public CreateNotificationCommand(Guid uniqueId, string title, string body)
        {
            UniqueId = uniqueId;
            Title = title;
            Body = body;
        }
    }
}