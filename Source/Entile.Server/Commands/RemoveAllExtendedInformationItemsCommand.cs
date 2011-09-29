using System;

namespace Entile.Server.Commands
{
    public class RemoveAllExtendedInformationItemsCommand : CommandBase
    {
        public Guid UniqueId { get; private set; }

        public RemoveAllExtendedInformationItemsCommand(Guid uniqueId)
        {
            UniqueId = uniqueId;
        }
    }
}