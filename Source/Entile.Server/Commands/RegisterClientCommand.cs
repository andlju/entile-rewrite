using System;
using System.Collections.Generic;

namespace Entile.Server.Commands
{
    public class RegisterClientCommand : CommandBase
    {
        public Guid ClientId { get; set; }
        public string NotificationChannel { get; set; }
        
        public RegisterClientCommand()
        {
            
        }

        public RegisterClientCommand(Guid clientId, string notificationChannel)
        {
            ClientId = clientId;
            NotificationChannel = notificationChannel;
        }
    }
}