using System;

namespace Entile.Server.Commands
{
    public class UnregisterClientCommand : CommandBase
    {
        public Guid UniqueId { get; private set; }

        public UnregisterClientCommand(Guid uniqueId)
        {
            UniqueId = uniqueId;
        }
    }
}