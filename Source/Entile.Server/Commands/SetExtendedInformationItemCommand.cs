using System;

namespace Entile.Server.Commands
{
    public class SetExtendedInformationItemCommand : CommandBase
    {
        public readonly Guid ClientId;
        public readonly string Key;
        public readonly string Value;

        public SetExtendedInformationItemCommand(Guid clientId, string key, string value)
        {
            ClientId = clientId;
            Key = key;
            Value = value;
        }
    }
}