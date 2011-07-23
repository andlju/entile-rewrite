using System;
using Entile.Server.Commands;

namespace Entile.Server
{
    public class SendNotificationCommand : CommandBase
    {
        public string UniqueId { get; private set; }

        public string Title { get; private set; }
        public string Body { get; private set; }

        public SendNotificationCommand(string uniqueId, string title, string body)
        {
            UniqueId = uniqueId;
            Title = title;
            Body = body;
        }
    }

}