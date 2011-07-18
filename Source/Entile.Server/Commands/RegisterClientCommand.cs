using System.Collections.Generic;

namespace Entile.Server.Commands
{
    public class RegisterClientCommand : CommandBase
    {
        public string UniqueId { get; private set; }
        public string NotificationChannel { get; private set; }

        public RegisterClientCommand(string uniqueId, string notificationChannel)
        {
            UniqueId = uniqueId;
            NotificationChannel = notificationChannel;
        }
    }
}