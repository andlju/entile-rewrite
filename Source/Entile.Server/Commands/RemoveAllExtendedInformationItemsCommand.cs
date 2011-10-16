using System;

namespace Entile.Server.Commands
{
    public class RemoveAllExtendedInformationItemsCommand : CommandBase
    {
        public readonly Guid ClientId;

        public RemoveAllExtendedInformationItemsCommand(Guid clientId)
        {
            ClientId = clientId;
        }
    }
}