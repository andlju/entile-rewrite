using System;

namespace Entile.Server.Commands
{
    public class RemoveExtendedInformationItemCommand : CommandBase
    {
        public Guid UniqueId { get; private set; }
        public string Key { get; private set; }

        public RemoveExtendedInformationItemCommand(Guid uniqueId, string key)
        {
            UniqueId = uniqueId;
            Key = key;
        }
    }
}