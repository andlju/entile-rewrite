using System;

namespace Entile.Server.Commands
{
    public class UnregisterClientCommand : CommandBase
    {
        public Guid ClientId;

        public UnregisterClientCommand()
        {
            
        }

        public UnregisterClientCommand(Guid clientId)
        {
            ClientId = clientId;
        }
    }
}