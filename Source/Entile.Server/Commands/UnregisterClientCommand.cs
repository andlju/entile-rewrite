using System;
using System.ComponentModel.DataAnnotations;

namespace Entile.Server.Commands
{
    public class UnregisterClientCommand : CommandBase
    {
        [Key]
        public Guid ClientId { get; set; }

        public UnregisterClientCommand()
        {
            
        }

        public UnregisterClientCommand(Guid clientId)
        {
            ClientId = clientId;
        }
    }
}