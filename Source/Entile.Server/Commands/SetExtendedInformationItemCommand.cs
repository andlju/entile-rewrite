using System;

namespace Entile.Server.Commands
{
    public class SetExtendedInformationItemCommand : CommandBase
    {
        public Guid UniqueId { get; private set; }
        public string Key { get; private set; }
        public string Value { get; private set; }

        public SetExtendedInformationItemCommand(Guid uniqueId, string key, string value)
        {
            UniqueId = uniqueId;
            Key = key;
            Value = value;
        }
    }
}