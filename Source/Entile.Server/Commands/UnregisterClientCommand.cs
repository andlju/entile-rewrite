using System;

namespace Entile.Server.Commands
{
    public class UnregisterClientCommand : CommandBase
    {
        public readonly Guid ClientId;

        public UnregisterClientCommand(Guid clientId)
        {
            ClientId = clientId;
        }
    }
}