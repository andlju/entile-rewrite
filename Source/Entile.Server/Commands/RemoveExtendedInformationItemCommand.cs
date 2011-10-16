using System;

namespace Entile.Server.Commands
{
    public class RemoveExtendedInformationItemCommand : CommandBase
    {
        public readonly Guid ClientId;
        public readonly string Key;

        public RemoveExtendedInformationItemCommand(Guid clientId, string key)
        {
            ClientId = clientId;
            Key = key;
        }
    }
}