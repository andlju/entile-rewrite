using System;
using System.Collections.Generic;

namespace Entile.Server.Commands
{
    public class RegisterClientCommand : CommandBase
    {
        public Guid UniqueId { get; private set; }
        public string NotificationChannel { get; private set; }

        public RegisterClientCommand(Guid uniqueId, string notificationChannel)
        {
            UniqueId = uniqueId;
            NotificationChannel = notificationChannel;
        }
    }
}