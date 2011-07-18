using System;

namespace Entile.Server.Commands
{
    public class CommandBase : ICommand
    {
        public long Timestamp { get; set; }

        public CommandBase()
        {
            Timestamp = DateTime.Now.Ticks;
        }
    }

    public class UnregisterClientCommand : CommandBase
    {
        public string UniqueId { get; private set; }

        public UnregisterClientCommand(string uniqueId)
        {
            UniqueId = uniqueId;
        }
    }
}