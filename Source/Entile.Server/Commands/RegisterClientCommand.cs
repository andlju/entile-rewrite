using System.Collections.Generic;

namespace Entile.Server.Commands
{
    public class RegisterClientCommand : ICommand
    {
        public string UniqueId { get; private set; }
        public string NotificationChannel { get; private set; }

        public IDictionary<string, string> ExtendedInformation { get; private set; }

        public RegisterClientCommand(string uniqueId, string notificationChannel, IDictionary<string, string> extendedInformation)
        {
            UniqueId = uniqueId;
            NotificationChannel = notificationChannel;
            ExtendedInformation = extendedInformation;
        }
    }
}