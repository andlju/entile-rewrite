using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entile.Server.Commands
{
    public class RegisterClientCommand : CommandBase
    {
        [Key]
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