using System;
using System.Collections.Generic;

namespace Entile.Server.Commands
{
    public class RegisterClientCommand : CommandBase
    {
        public readonly Guid ClientId;
        public readonly string NotificationChannel;

        public RegisterClientCommand(Guid clientId, string notificationChannel)
        {
            ClientId = clientId;
            NotificationChannel = notificationChannel;
        }
    }
}